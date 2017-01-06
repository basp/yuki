namespace Yuki
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.Contracts;
    using System.IO;
    using Commands;
    using NLog;
    using Optional;
    using Optional.Linq;

    using static Optional.Option;

    public class Migrator : IMigrator
    {
        private readonly ILogger log = LogManager.GetCurrentClassLogger();

        private readonly ISession session;
        private readonly ICommandFactory commandFactory;
        private readonly MigrateRequest request;

        public Migrator(
            ISession session,
            ICommandFactory commandFactory,
            MigrateRequest request)
        {
            Contract.Requires(session != null);
            Contract.Requires(commandFactory != null);
            Contract.Requires(request != null);

            this.session = session;
            this.commandFactory = commandFactory;
            this.request = request;
        }

        public Option<bool, Exception> RunMigrationScripts(
            string scriptFolder,
            string newVersion,
            int versionId,
            bool isOneTimeFolder = false,
            bool isEveryTimeFolder = false)
        {
            if (!Directory.Exists(scriptFolder))
            {
                this.log.Warn($"Script folder {scriptFolder} does not exist");
                return Some<bool, Exception>(true);
            }

            var scriptFiles = Directory.GetFiles(scriptFolder);
            return from res in this.RunScripts(versionId, scriptFiles, isOneTimeFolder, isEveryTimeFolder)
                   select true;

            // return res.Map(x => true);
        }

        public Option<GetVersionResponse, Exception> GetCurrentVersion()
        {
            var cmd = this.commandFactory.CreateGetVersionCommand(this.session);
            var res = cmd.Execute(new GetVersionRequest
            {
                Server = this.request.Server,
                RepositoryDatabase = this.request.RepositoryDatabase,
                RepositorySchema = this.request.RepositorySchema,
                RepositoryPath = this.request.RepositoryPath,
            });

            return res;
        }

        public Option<InsertVersionResponse, Exception> InsertNextVersion(
            string currentVersion,
            string nextVersion)
        {
            this.log.Info($"Migrating {this.request.Server} from version {currentVersion} to {nextVersion}");
            var cmd = this.commandFactory.CreateInsertVersionCommand(this.session);
            var res = cmd.Execute(new InsertVersionRequest
            {
                Server = this.request.Server,
                RepositoryDatabase = this.request.RepositoryDatabase,
                RepositorySchema = this.request.RepositorySchema,
                RepositoryPath = this.request.RepositoryPath,
                RepositoryVersion = nextVersion,
            });

            this.log.Info($"Versioning {this.request.Server} with version {nextVersion} based on {this.request.RepositoryPath}");
            return res;
        }

        public Option<ResolveVersionResponse, Exception> ResolveNextVersion()
        {
            var cmd = this.commandFactory.CreateResolveVersionCommand();
            var res = cmd.Execute(new ResolveVersionRequest
            {
                VersionFile = this.request.VersionFile,
            });

            return res;
        }

        private static bool IsEveryTimeScript(
            string scriptName,
            bool isEveryTimeScript)
        {
            var sanitized = scriptName.ToLowerInvariant();

            if (sanitized.StartsWith("everytime"))
            {
                return true;
            }

            if (sanitized.Contains(".everytime."))
            {
                return true;
            }

            return isEveryTimeScript;
        }

        private Option<bool, Exception> RunScripts(
            int versionId,
            string[] scriptFiles,
            bool isOneTimeFolder = false,
            bool isEveryTimeFolder = false)
        {
            foreach (var script in scriptFiles)
            {
                this.log.Info($"Running {script} on {this.request.Server}");

                var readFileReq = new ReadFileRequest
                {
                    Path = script,
                };

                var readFileCmd = this.commandFactory.CreateReadFileCommand();
                var readFileRes = readFileCmd.Execute(readFileReq);
                var relativePath = Utils.RelativePath(this.request.ProjectFolder, script);

                var runScriptRequest = new RunScriptRequest()
                {
                    Server = this.request.Server,
                    RepositoryDatabase = this.request.RepositoryDatabase,
                    RepositorySchema = this.request.RepositorySchema,
                };

                var res = from rf in readFileRes
                          from rs in this.RunScript(
                              versionId,
                              relativePath,
                              rf.Text,
                              rf.Hash,
                              isOneTimeFolder,
                              isEveryTimeFolder)
                          select new { ReadFileResult = rf, RunScriptResult = rs };

                res.MatchSome(x =>
                {
                    this.InsertScriptRun(
                        versionId,
                        script,
                        x.ReadFileResult.Text,
                        x.ReadFileResult.Hash,
                        isOneTimeFolder);
                });

                if (!res.HasValue)
                {
                    return res.Map(x => false);
                }
            }

            return Some<bool, Exception>(true);
        }

        private Option<bool, Exception> RunScript(
            int versionId,
            string scriptName,
            string sql,
            string hash,
            bool isOneTimeFolder = false,
            bool isEveryTimeFolder = false)
        {
            var scriptChangedRes = this
                .ScriptChangedSinceLastExecution(scriptName, hash)
                .Filter(changed => (isOneTimeFolder && !changed) || !isOneTimeFolder, () =>
                {
                    var msg = $"{scriptName} has changed since the last time it was run. By default, this is not allowed - scripts that run once should never change. To change this behavior to a warning set WarnOnOneTimeScriptChanges to true and run again.";
                    return new Exception(msg);
                });

            var scriptShouldRun = this.ScriptShouldRun(
                scriptName,
                sql,
                hash,
                isEveryTimeFolder);

            return from changed in scriptChangedRes
                   let stmts = StatementSplitter.Split(sql)
                   from shouldRun in scriptShouldRun
                   from res in this.ExecuteStatements(versionId, scriptName, sql, stmts)
                   select shouldRun ? res : false;
        }

        private Option<bool, Exception> ExecuteStatements(
            int versionId,
            string scriptFile,
            string sql,
            IEnumerable<string> stmts)
        {
            foreach (var s in stmts)
            {
                try
                {
                    var args = new Dictionary<string, object>();
                    var ct = CommandType.Text;
                    this.session.ExecuteNonQuery(s, args, ct);
                }
                catch (Exception ex)
                {
                    this.log.Error($"Error executing file '{scriptFile}': statement running was '{s}' ({ex.Message})");
                    this.session.RollbackTransaction();
                    this.InsertScriptRunError(scriptFile, sql, s, ex.Message);
                    return None<bool, Exception>(ex);
                }
            }

            return Some<bool, Exception>(true);
        }

        private Option<bool, Exception> ScriptShouldRun(
            string scriptName,
            string sql,
            string hash,
            bool isEveryTimeScript)
        {
            if (IsEveryTimeScript(scriptName, isEveryTimeScript))
            {
                return Some<bool, Exception>(true);
            }

            var scriptExecutedAlready = this.ScriptExecutedAlready(
                scriptName);

            var scriptChangedSinceLastExecution = this.ScriptChangedSinceLastExecution(
                scriptName,
                hash);

            return from changed in scriptChangedSinceLastExecution
                   from executed in scriptExecutedAlready
                   select !executed || changed;
        }

        private Option<bool, Exception> ScriptChangedSinceLastExecution(
            string scriptName,
            string newHash)
        {
            var currentHash = this.GetCurrentHash(scriptName)
                .Map(x => x.Hash)
                .ValueOr(string.Empty);

            if (string.IsNullOrWhiteSpace(currentHash))
            {
                return Some<bool, Exception>(false);
            }

            var changed = string.Compare(
                currentHash,
                newHash,
                StringComparison.OrdinalIgnoreCase) != 0;

            return Some<bool, Exception>(changed);
        }

        private Option<bool, Exception> ScriptExecutedAlready(
            string scriptName)
        {
            var hasScriptRunReq = new HasScriptRunRequest
            {
                RepositoryDatabase = this.request.RepositoryDatabase,
                RepositorySchema = this.request.RepositorySchema,
                ScriptName = scriptName,
            };

            var cmd = this.commandFactory.CreateHasScriptRunCommand(this.session);
            return cmd.Execute(hasScriptRunReq)
                .Map(x => x.HasRunAlready);
        }

        private Option<GetCurrentHashResponse, Exception> GetCurrentHash(
            string scriptName)
        {
            var cmd = this.commandFactory.CreateGetCurrentHashCommand(this.session);
            return cmd.Execute(new GetCurrentHashRequest
            {
                RepositoryDatabase = this.request.RepositoryDatabase,
                RepositorySchema = this.request.RepositorySchema,
                ScriptName = scriptName,
            });
        }

        private Option<InsertScriptRunResponse, Exception> InsertScriptRun(
            int versionId,
            string scriptName,
            string sql,
            string hash,
            bool isOneTimeScript = false)
        {
            var cmd = this.commandFactory.CreateInsertScriptRunCommand(this.session);
            var res = cmd.Execute(new InsertScriptRunRequest
            {
                VersionId = versionId,
                ScriptName = scriptName,
                Sql = sql,
                Hash = hash,
                IsOneTimeScript = isOneTimeScript,
                RepositoryDatabase = this.request.RepositoryDatabase,
                RepositorySchema = this.request.RepositorySchema,
            });

            return res;
        }

        private Option<InsertScriptRunErrorResponse, Exception> InsertScriptRunError(
            string scriptName,
            string sql,
            string sqlErrorPart,
            string errorMessage)
        {
            var cmd = this.commandFactory.CreateInsertScriptRunErrorCommand(this.session);
            var res = cmd.Execute(new InsertScriptRunErrorRequest
            {
                ScriptName = scriptName,
                Sql = sql,
                SqlErrorPart = sqlErrorPart,
                ErrorMessage = errorMessage,
            });

            return res;
        }
    }
}
