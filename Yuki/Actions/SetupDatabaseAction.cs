namespace Yuki.Actions
{
    using System;
    using System.Linq;
    using System.IO;
    using NLog;
    using Maybe;

    public enum SetupDatabaseResult
    {
        None,
        Error
    }

    public class SetupDatabaseAction : IAction<SetupDatabaseArgs, SetupDatabaseResult>
    {
        private enum IntermediateResult
        {
            None,
            Something,
            Error
        }

        private readonly ILogger log = LogManager.GetCurrentClassLogger();

        private readonly Context ctx;
        private readonly ISession session;
        private readonly IMigrator migrator;

        public SetupDatabaseAction(
            Context ctx,
            ISession session,
            IMigrator migrator)
        {
            this.ctx = ctx;
            this.session = session;
            this.migrator = migrator;
        }

        public IMaybeError<SetupDatabaseResult> Execute(SetupDatabaseArgs args)
        {
            var maybe = this.migrator.ForEachDatabase(this.SetupDatabase);
            return maybe.IsError
                ? MaybeError.Create(SetupDatabaseResult.Error, maybe.Exception)
                : MaybeError.Create(SetupDatabaseResult.None);
        }

        private void SetupDatabase(string folder)
        {
            IMaybeError<IntermediateResult> temp;

            var name = Path.GetFileName(folder);

            this.log.Info($"Creating database [{name}] if it doesn't exist");
            temp = this.CreateDatabase(name);
            if(temp.IsError)
            {
                throw temp.Exception;
            }

            var backupFile = GetMostRecentBackup(folder);
            if (string.IsNullOrEmpty(backupFile))
            {
                return;
            }

            this.log.Info($"Restoring [{name}] on {this.session} from {backupFile}");
            temp = this.RestoreDatabase(name, backupFile);
            if (temp.IsError)
            {
                throw temp.Exception;
            }
        }

        private static string GetMostRecentBackup(string folder)
        {
            return Directory.GetFiles(folder)
                .FirstOrDefault(x => x.EndsWith(
                    ".bak",
                    StringComparison.InvariantCultureIgnoreCase));
        }

        private IMaybeError<IntermediateResult> RestoreDatabase(
            string databaseName,
            string backupFile)
        {
            var restoreDatabaseAction = new RestoreDatabaseAction();
            var args = new RestoreDatabaseArgs()
            {
                BackupFile = backupFile,
                DatabaseName = databaseName,
                Server = this.session
            };

            var result = restoreDatabaseAction.Execute(args);
            return result.IsError
                ? MaybeError.Create(IntermediateResult.Error, result.Exception)
                : MaybeError.Create(IntermediateResult.None);
        }

        private IMaybeError<IntermediateResult> CreateDatabase(string name)
        {
            var createDatabaseAction = new CreateDatabaseAction(this.session);
            var args = new CreateDatabaseArgs()
            {
                Name = name,
                Server = this.session
            };

            var result = createDatabaseAction.Execute(args);
            return result.IsError
                ? MaybeError.Create(IntermediateResult.Error, result.Exception)
                : MaybeError.Create(IntermediateResult.None);
        }
    }
}
