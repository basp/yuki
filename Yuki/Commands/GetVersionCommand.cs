namespace Yuki.Commands
{
    using System;
    using System.Data;
    using System.Diagnostics.Contracts;
    using Optional;
    using Optional.Linq;

    using Req = GetVersionRequest;
    using Res = GetVersionResponse;

    public class GetVersionCommand
        : IGetVersionCommand
    {
        private readonly IRepositoryFactory repositoryFactory;

        public GetVersionCommand(
            IRepositoryFactory repositoryFactory)
        {
            Contract.Requires(repositoryFactory != null);
            this.repositoryFactory = repositoryFactory;
        }

        public Option<Res, Exception> Execute(Req req)
        {
            var repo = this.repositoryFactory.Create(
                req.RepositoryDatabase,
                req.RepositorySchema);

            return from v in repo.GetVersion(req.RepositoryPath)
                   select CreateResponse(req, v);
        }

        private static Res CreateResponse(
            Req req,
            string versionName)
        {
            return new Res
            {
                VersionName = versionName,
            };
        }
    }
}