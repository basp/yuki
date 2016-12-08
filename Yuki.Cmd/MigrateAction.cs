namespace Yuki.Cmd
{
    using System;
    using System.IO;
    using NLog;

    interface IDropFolder
    {
    }

    public class MigrateAction : IAction<MigrateArgs>
    {
        readonly ILogger log = LogManager.GetCurrentClassLogger();
        readonly Context ctx;

        public MigrateAction(Context ctx)
        {
            this.ctx = ctx;
        }
      
        public void Execute(MigrateArgs args)
        {
            var folders = this.ctx.Config.Folders;
            foreach(var f in folders)
            {
                this.log.Info($"{f.Name} ({f.Type})");
            }
        }
    }
}
