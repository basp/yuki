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

            if (!createDatabaseResult.HasValue)
            {
                // We have an error right here but just need to map the result type.
                return createDatabaseResult.Map(x => new Res());
            }

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
                return CreateResponse(req, None<string>());
            }

            var maybeBackup = this.backupFileProvider.GetFullPath();
            if (!maybeBackup.HasValue)
            {
                // For now, when a user requests a restore (which is currently an explicit
                // flag) we will bail when we can't find any backup file.
                return maybeBackup
                    .Map(x => new Res())
                    .MapException(x => new Exception($"No backup found in {req.Folder}", x));
            }

            // We should *never* get the TILT value, it's just there to satisfy the option.
            var backup = maybeBackup.ValueOr("TILT");

            var restoreDatabaseResult = this.RestoreDatabase(
                req.Server,
                req.Database,
                backup);

            if (!restoreDatabaseResult.HasValue)
            {
                // Same as before, satisfy the compiler as we do have an error value.
                return restoreDatabaseResult.Map(x => new Res());
            }

            this.log.Info($"Restored database [{req.Database}] on server [{req.Server}] using backup {backup}");
            return CreateResponse(req, Some(backup));
        }

        private static Option<Res, Exception> CreateResponse(Req req, Option<string> backup)
        {
            return Some<Res, Exception>(new Res()
            {
                Server = req.Server,
                Database = req.Database,
                Folder = req.Folder,
                Backup = backup.ValueOr(string.Empty),
            });
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
