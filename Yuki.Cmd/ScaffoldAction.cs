namespace Yuki.Cmd
{
    using NLog;
    using System.IO;
    using System.Linq;

    public class ScaffoldAction : IAction<ScaffoldArgs>
    {
        readonly ILogger log = LogManager.GetCurrentClassLogger();
        readonly Context ctx;

        public ScaffoldAction(Context ctx)
        {
            this.ctx = ctx;
        }

        public void Execute(ScaffoldArgs args)
        {
            var projectDir = this.ctx.ProjectDirectory;
            var wd = projectDir;
            var folders = this.ctx.Config.Folders;
            string msg;
            foreach (var f in folders)
            {
                Directory.SetCurrentDirectory(projectDir);
                log.Debug($"Working directory changed to {projectDir}");
                               
                var sd = Path.Combine(wd, f.Name);
                if (Directory.Exists(sd))
                {
                    msg = $"Directory {sd} already exists";
                    if (!args.Force)
                    {
                        log.Error(msg);
                        return;
                    }

                    log.Warn(msg);
                    log.Warn($"Continue anyway [forced]");
                }
                
                if(!Directory.Exists(sd))
                {
                    Directory.CreateDirectory(sd);
                    log.Info($"Created folder {sd}");
                }

                Directory.SetCurrentDirectory(sd);
                log.Debug($"Working directory changed to {sd}");
            }
        }
    }
}
