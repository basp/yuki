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

        public static bool IsEveryTimeScript(
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

        public Option<bool, Exception> RunScripts(
            string[] scriptFiles)
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

                var runScriptRequest = new RunScriptRequest();
                var res = from rf in readFileRes
                          from rs in this.RunScript(
                              runScriptRequest,
                              relativePath,
                              rf.Text,
                              rf.Hash)
                          select rs;

                res.MatchNone(err => this.log.Error($"Error executing file {script} ({err.Message})"));

                if (!res.HasValue)
                {
                    return res;
                }
            }

            return Some<bool, Exception>(true);
        }

        public Option<bool, Exception> RunScript(
           RunScriptRequest req,
           string scriptName,
           string sql,
           string hash)
        {
            var scriptChangedRes = this
                .ScriptChangedSinceLastExecution(req, scriptName, hash)
                .Filter(changed => (req.IsOneTimeFolder && !changed) || !req.IsOneTimeFolder, () =>
                {
                    var msg = $"{scriptName} has changed since the last time it was run. By default, this is not allowed - scripts that run once should never change. To change this behavior to a warning set WarnOnOneTimeScriptChanges to true and run again.";
                    return new Exception(msg);
                });

            var scriptShouldRun = this.ScriptShouldRun(
                req,
                scriptName,
                sql,
                hash,
                req.IsEveryTimeFolder);

            return from changed in scriptChangedRes
                   let stmts = StatementSplitter.Split(sql)
                   from shouldRun in scriptShouldRun
                   from res in this.ExecuteStatements(stmts)
                   select shouldRun ? res : false;
        }

        public Option<bool, Exception> ExecuteStatements(
            IEnumerable<string> stmts)
        {
            try
            {
                foreach (var s in stmts)
                {
                    var args = new Dictionary<string, object>();
                    var ct = CommandType.Text;
                    this.session.ExecuteNonQuery(s, args, ct);
                }

                return Some<bool, Exception>(true);
            }
            catch (Exception ex)
            {
                return None<bool, Exception>(ex);
            }
        }

        public Option<bool, Exception> ScriptShouldRun(
            RunScriptRequest req,
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
                req,
                scriptName);

            var scriptChangedSinceLastExecution = this.ScriptChangedSinceLastExecution(
                req,
                scriptName,
                hash);

            return from changed in scriptChangedSinceLastExecution
                   from executed in scriptExecutedAlready
                   select !executed || changed;
        }

        public Option<bool, Exception> ScriptChangedSinceLastExecution(
            RunScriptRequest req,
            string scriptName,
            string newHash)
        {
            var currentHashResult = this.GetCurrentHash(
                req,
                scriptName);

            var currentHash = currentHashResult
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

        public Option<bool, Exception> ScriptExecutedAlready(
            RunScriptRequest req,
            string scriptName)
        {
            var hasScriptRunReq = new HasScriptRunRequest
            {
                RepositoryDatabase = req.RepositoryDatabase,
                RepositorySchema = req.RepositorySchema,
                ScriptName = scriptName,
            };

            var cmd = this.commandFactory.CreateHasScriptRunCommand(this.session);
            return cmd.Execute(hasScriptRunReq)
                .Map(x => x.HasRunAlready);
        }

        public Option<GetCurrentHashResponse, Exception> GetCurrentHash(
            RunScriptRequest req,
            string scriptName)
        {
            var cmd = this.commandFactory.CreateGetCurrentHashCommand(this.session);
            return cmd.Execute(new GetCurrentHashRequest
            {
                RepositoryDatabase = req.RepositoryDatabase,
                RepositorySchema = req.RepositorySchema,
                ScriptName = scriptName,
            });
        }

        public Option<InsertScriptRunResponse, Exception> InsertScriptRun(
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

        public Option<InsertScriptRunErrorResponse, Exception> InsertScriptRunError(
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

            var cmd = this.commandFactory.CreateRunScriptsCommand(this.session, this);
            var res = cmd.Execute(new RunScriptsRequest
            {
                Server = this.request.Server,
                ProjectFolder = this.request.ProjectFolder,
                ScriptFolder = scriptFolder,
                VersionId = versionId,
                RepositoryDatabase = this.request.RepositoryDatabase,
                RepositorySchema = this.request.RepositorySchema,
                IsOneTimeFolder = isOneTimeFolder,
                IsEveryTimeFolder = isEveryTimeFolder,
                RepositoryVersion = newVersion,
            });

            return res.Map(x => true);
        }
    }
}
