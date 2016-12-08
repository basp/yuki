namespace Yuki.Cmd
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using NLog;

    public class InitAction : IAction<InitArgs>
    {
        readonly ILogger log = LogManager.GetCurrentClassLogger();
    
        public void Execute(InitArgs args)
        {
            var tasks = new LinkedList<Action>();
            var wd = string.IsNullOrWhiteSpace(args.Folder)
                ? Directory.GetCurrentDirectory()
                : Path.GetFullPath(args.Folder);

            tasks.AddLast(() =>
            {
                var inTargetDirectory = string.Equals(
                    wd,
                    Directory.GetCurrentDirectory(),
                    StringComparison.InvariantCultureIgnoreCase);

                if (!inTargetDirectory)
                {
                    if (!Directory.Exists(wd))
                    {
                        Directory.CreateDirectory(wd);
                        this.log.Info($"Created folder {wd}");
                    }

                    Directory.SetCurrentDirectory(wd);
                    this.log.Debug($"Working directory changed to {wd}");
                }
            });

            tasks.AddLast(() =>
            {
                var cf = Path.Combine(wd, Context.DefaultConfigFile);

                string msg;

                var hasEntries = Directory.EnumerateFileSystemEntries(wd).Any();
                if (hasEntries)
                {
                    msg = $"Working folder {wd} is not empty";

                    if (!args.Force)
                    {
                        log.Error(msg);
                        return;
                    }

                    log.Warn(msg);
                    log.Warn("Continue anyway [forced]");
                }

                if (File.Exists(cf))
                {
                    msg = $"Config file {cf} already exists";
                    if (!args.Overwrite)
                    {
                        log.Error(msg);
                        return;
                    }

                    log.Warn(msg);
                    log.Warn($"Overwriting {cf} [forced]");
                }

                var json = Utils.ReadEmbeddedString<Program>("Resources.yuki.default.json");
                File.WriteAllText(cf, json);
                log.Info($"Wrote {cf}");
            });

            Array.ForEach(tasks.ToArray(), t => t());
        }

        private static bool IsNestedProject(string cf, string twd)
        {
            var projectExists = File.Exists(cf);
            var tgtIsSiblingOfProjectDir = string.Equals(
                Path.GetDirectoryName(twd),
                Path.GetDirectoryName(cf),
                StringComparison.InvariantCultureIgnoreCase);

            return projectExists && tgtIsSiblingOfProjectDir;
        }
    }
}
