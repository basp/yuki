namespace Yuki.Commands
{
    using System;
    using System.Diagnostics.Contracts;
    using NLog;
    using Optional;

    using static Optional.Option;

    using Req = VersionRequest;
    using Res = VersionResponse;

    public class VersionCommand : ICommand<Req, Res, Exception>
    {
        private readonly ILogger log = LogManager.GetCurrentClassLogger();

        private readonly ISession session;
        private readonly IVersionProvider versionProvider;
        private readonly IIdentityProvider identity;

        public VersionCommand(
            ISession session,
            IVersionProvider versionProvider,
            IIdentityProvider identityProvider)
        {
            Contract.Requires(session != null);
            Contract.Requires(versionProvider != null);
            Contract.Requires(identityProvider != null);

            this.session = session;
            this.versionProvider = versionProvider;
            this.identity = identityProvider;
        }

        public Option<Res, Exception> Execute(Req request)
        {
            var repo = new SqlRepository(this.session, request);
            var currentVersion = repo.GetVersion(request.RepositoryPath);
            var newVersion = this.versionProvider.Resolve();
            if (!newVersion.HasValue)
            {
                return newVersion.Map(x => new Res());
            }

            var insertVersionRequest = new InsertVersionRequest()
            {
                Server = request.Server,
                RepositoryDatabase = request.RepositoryDatabase,
                RepositorySchema = request.RepositorySchema,
                RepositoryPath = request.RepositoryPath,
                VersionName = newVersion.ValueOr("TILT"),
            };

            var insertVersionCmd = new InsertVersionCommand(
                this.session,
                this.identity);

            var insertVersionResponse = insertVersionCmd
                .Execute(insertVersionRequest);

            if (!insertVersionResponse.HasValue)
            {
                return insertVersionResponse.Map(x => new Res());
            }

            // Running into 0x1DEDBABE here is a bug.
            // Eiter in Yuki or in your migration process.
            var versionId = insertVersionResponse.Match(
                some => some.VersionId,
                none => 0x1DEDBABE);

            var res = new Res()
            {
                Server = request.Server,
                RepositoryDatabase = request.RepositoryDatabase,
                RepositorySchema = request.RepositorySchema,
                CurrentVersion = newVersion.ValueOr("TILT"),
                PreviousVersion = currentVersion.ValueOr("TILT"),
                RepositoryPath = request.RepositoryPath,
                VersionFile = request.VersionFile,
                VersionId = versionId,
            };

            return Some<Res, Exception>(res);
        }
    }
}
