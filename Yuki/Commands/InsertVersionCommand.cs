namespace Yuki.Commands
{
    using System;
    using System.Diagnostics.Contracts;
    using Optional;

    using static Optional.Option;

    using Req = InsertVersionRequest;
    using Res = InsertVersionResponse;

    public class InsertVersionCommand : ICommand<Req, Res, Exception>
    {
        private readonly ISession session;
        private readonly IIdentityProvider identity;

        public InsertVersionCommand(ISession session, IIdentityProvider identity)
        {
            Contract.Requires(session != null);
            Contract.Requires(identity != null);

            this.session = session;
            this.identity = identity;
        }

        public Option<Res, Exception> Execute(Req req)
        {
            req.EnteredBy = this.identity
                .GetCurrent()
                .ValueOr(Environment.MachineName);

            var repository = new SqlRepository(this.session, req);
            var result = repository.InsertVersion(req);
            return result
                .Map(x => CreateResponse(x, req))
                .MapException(x => (Exception)x);
        }

        private static Res CreateResponse(int versionId, Req req)
        {
            return new Res()
            {
                VersionId = versionId,
                VersionName = req.VersionName,
                RepositoryPath = req.RepositoryPath,
                EnteredBy = req.EnteredBy,
                Server = req.Server,
                Database = req.RepositoryDatabase,
                Schema = req.RepositorySchema,
            };
        }
    }
}
