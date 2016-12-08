namespace Yuki.Actions
{
    using System.IO;
    using NLog;
    using Maybe;

    public enum RestoreResult
    {
        None,
        Error
    }

    public class RestoreAction : IAction<RestoreArgs, RestoreResult>
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

        public IMaybeError<RestoreResult> Execute(RestoreArgs args)
        {
            var maybe = this.migrator.ForEachDatabase(x =>
            {
                this.log.Info($"=> ${x}");
            });

            return maybe.IsError 
                ? MaybeError.Create(RestoreResult.Error, maybe.Exception)
                : MaybeError.Create(RestoreResult.None);
        }

        private void SetupDatabase(string folder)
        {
            var name = Path.GetFileName(folder);
            this.log.Info($"Creating database [{name}] if it doesn't exist");


        }
    }
}
