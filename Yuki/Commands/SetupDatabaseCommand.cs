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
            Contract.Requires(restoreDatabaseCommand != null);

            this.session = session;
            this.backupFileProvider = backupFileProvider;
            this.restoreDatabaseCommand = restoreDatabaseCommand;
            this.createDatabaseCommand = createDatabaseCommand;
        }

        public Option<Res, Exception> Execute(Req req)
        {
            this.log.Info($"Creating database [{req.Database}] on server {req.Server} if it does not exist");

            var createDatabaseResult = this.CreateDatabaseCommand(
                req.Server,
                req.Database);

            createDatabaseResult.MatchSome(x =>
            {
                var msg = x.Created
                    ? $"Created database [{req.Database}]"
                    : $"Database [{req.Database}] already exists";

                this.log.Info(msg);
            });

            if (!req.Restore)
            {
                this.log.Info($"Skipping restore [default]");
                return Some<Res, Exception>(CreateResponse(req, string.Empty));
            }

            var result = createDatabaseResult
                .FlatMap(x => this.backupFileProvider.TryFindIn(req.Folder))
                .MapException(x => new Exception($"No backup found in {req.Folder}", x))
                .FlatMap(x => this.RestoreDatabase(req.Server, req.Database, x))
                .Map(x => CreateResponse(req, x.Backup));

            result.MatchSome(x =>
            {
                var msg = $"Restored database [{req.Database}] on server [{req.Server}] using backup {x.Backup}";
                this.log.Info(msg);
            });

            return result;
        }

        private static Res CreateResponse(Req req, string backup)
        {
            return new Res()
            {
                Server = req.Server,
                Database = req.Database,
                Folder = req.Folder,
                Backup = backup,
            };
        }

        private Option<CreateDatabaseResponse, Exception> CreateDatabaseCommand(
            string server,
            string database)
        {
            var req = new CreateDatabaseRequest()
            {
                Server = server,
                Database = database,
            };

            return this.createDatabaseCommand.Execute(req);
        }

        private Option<RestoreDatabaseResponse, Exception> RestoreDatabase(
            string server,
            string database,
            string backup)
        {
            var req = new RestoreDatabaseRequest()
            {
                Server = server,
                Database = database,
                Backup = backup,
            };

            return this.restoreDatabaseCommand.Execute(req);
        }
    }
}
