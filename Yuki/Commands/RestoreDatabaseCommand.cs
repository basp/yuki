namespace Yuki.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.Contracts;
    using Optional;
    using Optional.Linq;
    using Serilog;
    using Templates;

    using Req = RestoreDatabaseRequest;
    using Res = RestoreDatabaseResponse;

    public class RestoreDatabaseCommand : IRestoreDatabaseCommand
    {
        private readonly IDatabaseFactory databaseFactory;

        public RestoreDatabaseCommand(
            IDatabaseFactory databaseFactory)
        {
            Contract.Requires(databaseFactory != null);

            this.databaseFactory = databaseFactory;
        }

        public Option<Res, Exception> Execute(Req req)
        {
            return this.RestoreDatabase(req)
                .Map(x => CreateResponse(req));
        }

        private static Res CreateResponse(Req req)
        {
            return new Res
            {
                Restored = true,
                Backup = req.Backup,
            };
        }

        private Option<bool, Exception> RestoreDatabase(Req req)
        {
            Log.Information(
                "Restoring {Database} on {Server} using backup {BackupFile}",
                req.Database,
                req.Server,
                req.Backup);

            var database = this.databaseFactory.Create(req.Database);
            return database.Restore(req.Backup);
        }
    }
}