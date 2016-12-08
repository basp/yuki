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
            var folders = this.ctx.Config.folders ?? new string[0];
            foreach(var f in folders)
            {
                this.log.Info($"{f.name} ({f.type})");
            }
        }
    }
}
