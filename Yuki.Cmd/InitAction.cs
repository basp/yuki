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
            string msg;

            var tasks = new LinkedList<Action>();

            var wd = string.IsNullOrWhiteSpace(args.Folder)
                ? Directory.GetCurrentDirectory()
                : Path.GetFullPath(args.Folder);

            var pd = Directory.GetParent(wd).FullName;

            if (File.Exists(Path.Combine(pd, Context.DefaultConfigFile)))
            {
                var msgs = new string[] {
                    $"The {pd} folder already contains a {Context.DefaultConfigFile} file.",
                    $"I was just trying to initialize a project at {Path.Combine(wd, Context.DefaultConfigFile)}",
                    $"but it looks like you're trying to initialize a new project inside a",
                    $"folder that is already a Yuki project folder; nested projects are not supported",
                    $"without forcing them."
                };

                if (!args.Force)
                {
                    Array.ForEach(msgs, this.log.Error);
                    return;
                }

                Array.ForEach(msgs, this.log.Warn);
                this.log.Warn("Continue anyway [force]");
            }

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
                    log.Warn("Continue anyway [force]");
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
                    log.Warn($"Overwriting {cf} [overwrite]");
                }

                var json = Utils.ReadEmbeddedString<Program>("Resources.yuki.default.json");
                File.WriteAllText(cf, json);
                log.Info($"Wrote {cf}");
            });

            Array.ForEach(tasks.ToArray(), t => t());
        }
    }
}
