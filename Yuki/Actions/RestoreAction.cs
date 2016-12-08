namespace Yuki.Actions
{
    using System.IO;
    using NLog;

    public class RestoreAction : IAction<RestoreArgs>
    {
        private readonly ILogger log = LogManager.GetCurrentClassLogger();

        private readonly Context ctx;
        private readonly ISession session;
        private readonly IMigrator migrator;

        public RestoreAction(
            Context ctx,
            ISession session,
            IMigrator migrator)
        {
            this.ctx = ctx;
            this.session = session;
            this.migrator = migrator;
        }

        public IMaybeError Execute(RestoreArgs args)
        {
            var maybe = this.migrator.ForEachDatabase(x =>
            {
                this.log.Info($"=> ${x}");
            });

            return maybe.IsError ? maybe : new MaybeError();
        }

        private void SetupDatabase(string folder)
        {
            var name = Path.GetFileName(folder);
            this.log.Info($"Creating database [{name}] if it doesn't exist");

            
        }
    }
}
