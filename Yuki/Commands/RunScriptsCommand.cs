namespace Yuki.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.Contracts;
    using System.IO;
    using NLog;
    using Optional;
    using Optional.Linq;

    using static Optional.Option;

    using Req = RunScriptsRequest;
    using Res = RunScriptsResponse;

    public class RunScriptsCommand : IRunScriptsCommand
    {
        private readonly ILogger log = LogManager.GetCurrentClassLogger();

        private readonly ISession session;
        private readonly IReadFileCommand readFileCommand;
        private readonly IHasScriptRunCommand hasScriptRunCommand;
        private readonly IGetCurrentHashCommand getCurrentHashCommand;

        public RunScriptsCommand(
            ISession session,
            IReadFileCommand readFileCommand,
            IHasScriptRunCommand hasScriptRunCommand,
            IGetCurrentHashCommand getCurrentHashCommand)
        {
            Contract.Requires(session != null);
            Contract.Requires(readFileCommand != null);
            Contract.Requires(hasScriptRunCommand != null);
            Contract.Requires(getCurrentHashCommand != null);

            this.session = session;
            this.readFileCommand = readFileCommand;
            this.hasScriptRunCommand = hasScriptRunCommand;
            this.getCurrentHashCommand = getCurrentHashCommand;
        }

        public Option<Res, Exception> Execute(Req req)
        {
            if (!Directory.Exists(req.ScriptFolder))
            {
                this.log.Warn($"Script directory {req.ScriptFolder} doesn't exist");
                return Some<Res, Exception>(CreateResponse(req));
            }

            var scripts = Directory.GetFiles(req.ScriptFolder, "*.sql");
            var res = this.RunScripts(req, scripts);
            return res.Map(x => CreateResponse(req));
        }

        private static Res CreateResponse(Req req)
        {
            return new Res
            {
            };
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
            Req req,
            string[] scriptFiles)
        {
            foreach (var script in scriptFiles)
            {
                this.log.Info($"Running {script} on {req.Server}");

                var readFileReq = new ReadFileRequest
                {
                    Path = script,
                };

                var readFileRes = this.readFileCommand.Execute(readFileReq);
                var relativePath = Utils.RelativePath(req.ProjectFolder, script);

                var res = from rf in readFileRes
                          from rs in this.RunScript(req, relativePath, rf.Text, rf.Hash)
                          select rs;

                res.MatchNone(
                    err => this.log.Error($"Error executing file {script} ({err.Message})"));

                if (!res.HasValue)
                {
                    return res;
                }
            }

            return Some<bool, Exception>(true);
        }

        private Option<bool, Exception> RunScript(
            Req req,
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

        private Option<bool, Exception> ExecuteStatements(
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

        private Option<bool, Exception> ScriptShouldRun(
            Req req,
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

        private Option<bool, Exception> ScriptChangedSinceLastExecution(
            Req req,
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

        private Option<bool, Exception> ScriptExecutedAlready(
            Req req,
            string scriptName)
        {
            var hasScriptRunReq = new HasScriptRunRequest
            {
                RepositoryDatabase = req.RepositoryDatabase,
                RepositorySchema = req.RepositorySchema,
                ScriptName = scriptName,
            };

            return this.hasScriptRunCommand
                .Execute(hasScriptRunReq)
                .Map(x => x.HasRunAlready);
        }

        private Option<GetCurrentHashResponse, Exception> GetCurrentHash(
            Req req,
            string scriptName)
        {
            return this.getCurrentHashCommand.Execute(new GetCurrentHashRequest
            {
                RepositoryDatabase = req.RepositoryDatabase,
                RepositorySchema = req.RepositorySchema,
                ScriptName = scriptName,
            });
        }

        private Option<ReadFileResponse, Exception> ReadFile(string path)
        {
            var res = this.readFileCommand.Execute(new ReadFileRequest
            {
                Path = path,
            });

            return res;
        }

        private Option<string[], Exception> GetScriptFiles(string folder)
        {
            try
            {
                var scripts = Directory.GetFiles(folder, "*.sql");
                return Some<string[], Exception>(scripts);
            }
            catch (Exception ex)
            {
                return None<string[], Exception>(ex);
            }
        }
    }
}