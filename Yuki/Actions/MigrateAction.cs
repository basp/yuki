namespace Yuki.Actions
{
    using Maybe;
    using NLog;
  
    public enum MigrateResult
    {
        None
    }

    public interface IDropFolder
    {
    }

    public class MigrateAction : IAction<MigrateArgs, MigrateResult>
    {
        private readonly ILogger log = LogManager.GetCurrentClassLogger();
        private readonly Context ctx;

        public MigrateAction(Context ctx)
        {
            this.ctx = ctx;
        }

        public IMaybeError<MigrateResult> Execute(MigrateArgs args)
        {
            var folders = this.ctx.Config.Folders;
            foreach (var f in folders)
            {
                this.log.Info($"{f.Name} ({f.Type})");
            }

            return MaybeError.Create(MigrateResult.None);
        }
    }
}
