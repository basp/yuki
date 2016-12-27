namespace Yuki.Commands
{
    using System;
    using System.Diagnostics.Contracts;
    using System.IO;
    using NLog;
    using Optional;
    using Optional.Linq;

    using static Optional.Option;

    public class RunScriptsCommand : IRunScriptsCommand
    {
        private readonly ILogger log = LogManager.GetCurrentClassLogger();

        private readonly ISession session;
        private readonly IReadFileCommand readFileCommand;

        public RunScriptsCommand(
            ISession session,
            IReadFileCommand readFileCommand)
        {
            Contract.Requires(session != null);
            Contract.Requires(readFileCommand != null);

            this.session = session;
            this.readFileCommand = readFileCommand;
        }

        public Option<RunScriptsResponse, Exception> Execute(RunScriptsRequest req)
        {
            var res = from scripts in this.GetScriptFiles(req.Folder)
                      select scripts;

            throw new NotImplementedException();
        }

        private Option<bool, Exception> RunScripts(string[] scripts)
        {
            foreach (var script in scripts)
            {
            }

            throw new NotImplementedException();
        }

        private Option<bool, Exception> HasScriptRun(string scriptName)
        {
            throw new NotImplementedException();
        }

        private Option<string[], Exception> GetScriptFiles(string folder)
        {
            try
            {
                if (!Directory.Exists(folder))
                {
                    this.log.Warn("Script directory {0} doesn't exist", folder);
                    return Some<string[], Exception>(new string[0]);
                }

                var scripts = Directory.GetFiles(folder, "*.sql");
                return Some<string[], Exception>(scripts);
            }
            catch (Exception ex)
            {
                return None<string[], Exception>(ex);
            }
        }

        private Option<ReadFileResponse, Exception> ReadFile(string path)
        {
            var res = this.readFileCommand.Execute(new ReadFileRequest
            {
                Path = path,
            });

            return res;
        }
    }
}
