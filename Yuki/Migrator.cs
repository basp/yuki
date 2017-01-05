namespace Yuki
{
    using System;
    using System.Diagnostics.Contracts;
    using System.IO;
    using Commands;
    using NLog;
    using Optional;

    using static Optional.Option;

    public class Migrator : IMigrator
    {
        private readonly ILogger log = LogManager.GetCurrentClassLogger();

        private readonly ISession session;
        private readonly ICommandFactory commandFactory;
        private readonly MigrateRequest request;

        public Migrator(
            ISession session,
            ICommandFactory commandFactory,
            MigrateRequest request)
        {
            Contract.Requires(session != null);
            Contract.Requires(commandFactory != null);
            Contract.Requires(request != null);

            this.session = session;
            this.commandFactory = commandFactory;
            this.request = request;
        }

        public Option<GetVersionResponse, Exception> GetCurrentVersion()
        {
            var cmd = this.commandFactory.CreateGetVersionCommand(this.session);
            var res = cmd.Execute(new GetVersionRequest
            {
                Server = this.request.Server,
                RepositoryDatabase = this.request.RepositoryDatabase,
                RepositorySchema = this.request.RepositorySchema,
                RepositoryPath = this.request.RepositoryPath,
            });

            return res;
        }

        public Option<InsertVersionResponse, Exception> InsertNextVersion(
            string currentVersion,
            string nextVersion)
        {
            this.log.Info($"Migrating {this.request.Server} from version {currentVersion} to {nextVersion}");
            var cmd = this.commandFactory.CreateInsertVersionCommand(this.session);
            var res = cmd.Execute(new InsertVersionRequest
            {
                Server = this.request.Server,
                RepositoryDatabase = this.request.RepositoryDatabase,
                RepositorySchema = this.request.RepositorySchema,
                RepositoryPath = this.request.RepositoryPath,
                RepositoryVersion = nextVersion,
            });

            this.log.Info($"Versioning {this.request.Server} with version {nextVersion} based on {this.request.RepositoryPath}");
            return res;
        }

        public Option<ResolveVersionResponse, Exception> ResolveNextVersion()
        {
            var cmd = this.commandFactory.CreateResolveVersionCommand();
            var res = cmd.Execute(new ResolveVersionRequest
            {
                VersionFile = this.request.VersionFile,
            });

            return res;
        }

        public Option<bool, Exception> RunMigrationScripts(
            string scriptFolder,
            string newVersion,
            int versionId,
            bool isOneTimeFolder = false,
            bool isEveryTimeFolder = false)
        {
            if (!Directory.Exists(scriptFolder))
            {
                this.log.Warn($"Script folder {scriptFolder} does not exist");
                return Some<bool, Exception>(true);
            }

            var cmd = this.commandFactory.CreateRunScriptsCommand(this.session);
            var res = cmd.Execute(new RunScriptsRequest
            {
                Server = this.request.Server,
                ProjectFolder = this.request.ProjectFolder,
                ScriptFolder = scriptFolder,
                VersionId = versionId,
                RepositoryDatabase = this.request.RepositoryDatabase,
                RepositorySchema = this.request.RepositorySchema,
                IsOneTimeFolder = isOneTimeFolder,
                IsEveryTimeFolder = isEveryTimeFolder,
                RepositoryVersion = newVersion,
            });

            return res.Map(x => true);
        }
    }
}
