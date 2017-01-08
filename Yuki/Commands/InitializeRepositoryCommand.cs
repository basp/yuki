namespace Yuki.Commands
{
    using System;
    using System.Diagnostics.Contracts;
    using Optional;

    using Req = InitializeRepositoryRequest;
    using Res = InitializeRepositoryResponse;

    public class InitializeRepositoryCommand : IInitializeRepositoryCommand
    {
        private readonly IRepositoryFactory repositoryFactory;

        public InitializeRepositoryCommand(
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

            return repo.Initialize().Map(x => CreateResponse(req));
        }

        private static Res CreateResponse(Req req)
        {
            return new Res
            {
                Server = req.Server,
                RepositoryDatabase = req.RepositoryDatabase,
                RepositorySchema = req.RepositorySchema,
            };
        }
    }
}
