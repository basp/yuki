namespace Yuki.Cmd
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public class ScaffoldAction : IAction<ScaffoldArgs>
    {
        readonly Context ctx;

        public ScaffoldAction(Context ctx)
        {
            this.ctx = ctx;
        }

        public void Execute(ScaffoldArgs args)
        {
            var tasks = new LinkedList<Action>();
            var folders = this.ctx.Config.folders ?? new string[0];
            foreach(var f in folders)
            {
                tasks.AddLast(() =>
                {
                });
            }
        }
    }
}
