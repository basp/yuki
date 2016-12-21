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
            var insertVersionRequest = new InsertVersionRequest()
            {
                Server = request.Server,
                RepositoryDatabase = request.RepositoryDatabase,
                RepositorySchema = request.RepositorySchema,
                RepositoryPath = request.RepositoryPath,
            };

            var insertVersionCmd = new InsertVersionCommand(
               this.session,
               this.identity);

            var res = new Res()
            {
                Server = request.Server,
                RepositoryDatabase = request.RepositoryDatabase,
                RepositorySchema = request.RepositorySchema,
                RepositoryPath = request.RepositoryPath,
                VersionFile = request.VersionFile,
            };

            var repo = new SqlRepository(this.session, request);
            return repo.GetVersion(request.RepositoryPath)
                .FlatMap(x =>
                {
                    res.PreviousVersion = x;
                    return this.versionProvider.Resolve();
                })
                .FlatMap(x =>
                {
                    res.CurrentVersion = x;
                    insertVersionRequest.VersionName = x;
                    return insertVersionCmd.Execute(insertVersionRequest);
                })
                .Map(x =>
                {
                    res.VersionId = x.VersionId;
                    return res;
                });
        }
    }
}
