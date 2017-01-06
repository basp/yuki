namespace Yuki.Commands
{
    using System;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Linq;
    using Optional;
    using Optional.Linq;
    using Serilog;

    using static Optional.Option;

    using Req = SetupDatabaseRequest;
    using Res = SetupDatabaseResponse;

    public class SetupDatabaseCommand : ISetupDatabaseCommand
    {
        private readonly ICreateDatabaseCommand createDatabaseCmd;
        private readonly IRestoreDatabaseCommand restoreDatabaseCmd;

        public SetupDatabaseCommand(
            ICreateDatabaseCommand createDatabaseCmd,
            IRestoreDatabaseCommand restoreDatabaseCmd)
        {
            Contract.Requires(createDatabaseCmd != null);
            Contract.Requires(restoreDatabaseCmd != null);

            this.createDatabaseCmd = createDatabaseCmd;
            this.restoreDatabaseCmd = restoreDatabaseCmd;
        }

        public Option<Res, Exception> Execute(Req req)
        {
            var database = Path.GetFileName(req.Folder);
            var createDatabaseRes = this.CreateDatabase(req, database);
            createDatabaseRes.MatchSome(x =>
            {
                var msg = x.Created
                    ? "Created database {Database} on server {Server}"
                    : "Database {Database} already exists on server {Server}";

                Log.Information(msg, database, req.Server);
            });

            if (!req.Restore)
            {
                Log.Information("Skipping restore");
                return createDatabaseRes.Map(x => CreateResponse(req, x));
            }

            // For now we fail when the restore flag is set and we can't
            // find a backup file to restore from. Whe might want to
            // reconsider this behavior.
            return from fi in TryFindBackup(req.Folder)
                   from x in createDatabaseRes
                   from y in this.RestoreDatabase(req.Server, fi.FullName, database)
                   select CreateResponse(req, x, y.Restored, y.Backup);
        }

        private static Res CreateResponse(
            Req req,
            CreateDatabaseResponse createDatabaseRes,
            bool restored = false,
            string backup = "")
        {
            return new Res
            {
                Server = req.Server,
                Folder = req.Folder,
                Database = createDatabaseRes.Database,
                Created = createDatabaseRes.Created,
                Restored = restored,
                Backup = backup,
            };
        }

        private static Option<FileInfo, Exception> TryFindBackup(string folder)
        {
            try
            {
                var errorMsg = $"No backup found in {folder}";
                return Directory.GetFiles(folder, "*.bak", SearchOption.TopDirectoryOnly)
                    .FirstOrDefault()
                    .SomeNotNull()
                    .Map(x => new FileInfo(x))
                    .WithException(() => new Exception(errorMsg));
            }
            catch (Exception ex)
            {
                return None<FileInfo, Exception>(ex);
            }
        }

        private Option<CreateDatabaseResponse, Exception> CreateDatabase(
            Req req,
            string database)
        {
            return this.createDatabaseCmd.Execute(new CreateDatabaseRequest
            {
                Server = req.Server,
                Database = database,
            });
        }

        private Option<RestoreDatabaseResponse, Exception> RestoreDatabase(
            string server,
            string backup,
            string database)
        {
            return this.restoreDatabaseCmd.Execute(new RestoreDatabaseRequest
            {
                Server = server,
                Database = database,
                Backup = backup,
            });
        }
    }
}