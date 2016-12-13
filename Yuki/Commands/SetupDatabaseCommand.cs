namespace Yuki.Commands
{
    using System;
    using System.Diagnostics.Contracts;
    using NLog;
    using Optional;

    using static Optional.Option;

    using Req = SetupDatabaseRequest;
    using Res = SetupDatabaseResponse;

    public class SetupDatabaseCommand : ICommand<Req, Res, Exception>
    {
        private readonly ILogger log = LogManager.GetCurrentClassLogger();
        private readonly ISession session;
        private readonly IBackupFileProvider backupFileProvider;
        private readonly ICommand<CreateDatabaseRequest, CreateDatabaseResponse, Exception> createDatabaseCommand;
        private readonly ICommand<RestoreDatabaseRequest, RestoreDatabaseResponse, Exception> restoreDatabaseCommand;

        public SetupDatabaseCommand(
            ISession session,
            IBackupFileProvider backupFileProvider,
            ICommand<CreateDatabaseRequest, CreateDatabaseResponse, Exception> createDatabaseCommand,
            ICommand<RestoreDatabaseRequest, RestoreDatabaseResponse, Exception> restoreDatabaseCommand)
        {
            Contract.Requires(session != null);
            Contract.Requires(backupFileProvider != null);
            Contract.Requires(createDatabaseCommand != null);

            this.session = session;
            this.backupFileProvider = backupFileProvider;
            this.createDatabaseCommand = createDatabaseCommand;
        }

        public Option<Res, Exception> Execute(Req request)
        {
            this.log.Info($"Creating database [{request.Database}] on server {request.Server} if it does not exist");

            var createDatabaseResult = this.CreateDatabaseCommand(request);
            if (!createDatabaseResult.HasValue)
            {
                return createDatabaseResult.Map(x => Res.Done);
            }

            createDatabaseResult.MatchSome(x =>
            {
                var msg = x.Created
                    ? $"Created database [{request.Database}]"
                    : $"Database [{request.Database}] already exists";

                this.log.Info(msg);
            });

            if (!request.Restore)
            {
                this.log.Info($"Skipping restore [default]");
                return Some<Res, Exception>(Res.Done);
            }

            var backup = this.backupFileProvider.GetFullPath();
            if (!backup.HasValue)
            {
                return backup
                    .Map(x => Res.Done)
                    .MapException(x => new Exception($"No backup found in {request.Folder}", x));
            }

            return Some<Res, Exception>(Res.Done);
        }

        private Option<CreateDatabaseResponse, Exception> CreateDatabaseCommand(Req request)
        {
            var createDatabaseRequest = new CreateDatabaseRequest()
            {
                Database = request.Database,
                Server = request.Server,
            };

            return this.createDatabaseCommand.Execute(createDatabaseRequest);
        }

        private Option<RestoreDatabaseResponse, Exception> RestoreDatabase(string backupFile)
        {
            return None<RestoreDatabaseResponse, Exception>(new NotImplementedException());
        }
    }
}
