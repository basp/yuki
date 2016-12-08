namespace Yuki.Actions
{
    using System;
    using System.IO;
    using NLog;
 
    public class ScaffoldAction : IAction<ScaffoldArgs>
    {
        private readonly ILogger log = LogManager.GetCurrentClassLogger();
        private readonly Context ctx;

        public ScaffoldAction(Context ctx)
        {
            this.ctx = ctx;
        }

        public IMaybeError Execute(ScaffoldArgs args)
        {
            var projectDir = this.ctx.ProjectDirectory;
            var wd = projectDir;
            var folders = this.ctx.Config.Folders;

            string msg;
            foreach (var f in folders)
            {
                Directory.SetCurrentDirectory(projectDir);
                this.log.Debug($"Working directory changed to {projectDir}");

                var sd = Path.Combine(wd, f.Name);
                if (Directory.Exists(sd))
                {
                    msg = $"Directory {sd} already exists";
                    if (!args.Force)
                    {
                        this.log.Error(msg);
                        var ex = new Exception(msg);
                        return new Error(ex);
                    }

                    this.log.Warn(msg);
                    this.log.Warn($"Continue anyway [force]");
                }

                if (!Directory.Exists(sd))
                {
                    Directory.CreateDirectory(sd);
                    this.log.Info($"Created folder {sd}");
                }

                Directory.SetCurrentDirectory(sd);
                this.log.Debug($"Working directory changed to {sd}");
            }

            return new Ok();
        }
    }
}
