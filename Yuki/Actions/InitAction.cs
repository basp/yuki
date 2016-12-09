namespace Yuki.Actions
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Maybe;
    using NLog;
  
    public enum InitResult
    {
        None
    }

    public class InitAction : IAction<InitArgs, InitResult>
    {
        private readonly ILogger log = LogManager.GetCurrentClassLogger();

        public IMaybeError<InitResult> Execute(InitArgs args)
        {
            string msg;

            var tasks = new LinkedList<Action>();

            var wd = string.IsNullOrWhiteSpace(args.Folder)
                ? Directory.GetCurrentDirectory()
                : Path.GetFullPath(args.Folder);

            var pd = Directory.GetParent(wd).FullName;

            if (File.Exists(Path.Combine(pd, Default.ConfigFile)))
            {
                var msgs = new string[] 
                {
                    $"The {pd} folder already contains a {Default.ConfigFile} file.",
                    $"I was just trying to initialize a project at {Path.Combine(wd, Default.ConfigFile)}",
                    $"but it looks like you're trying to initialize a new project inside a",
                    $"folder that is already a Yuki project folder; nested projects are not supported",
                    $"without forcing them."
                };

                if (!args.Force)
                {
                    Array.ForEach(msgs, this.log.Error);
                    var ex = new Exception();
                    return MaybeError.Create(InitResult.None, ex);
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
                var cf = Path.Combine(wd, Default.ConfigFile);
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

                var json = Utils.ReadEmbeddedString<Program>("yuki.default.json");
                File.WriteAllText(cf, json);
                log.Info($"Wrote {cf}");
            });

            Array.ForEach(tasks.ToArray(), t => t());
            return MaybeError.Create(InitResult.None);
        }
    }
}
