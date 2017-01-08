namespace Yuki.Commands
{
    using System;
    using System.Data;
    using System.Diagnostics.Contracts;
    using AutoMapper;
    using Optional;
    using Optional.Linq;

    using Req = GetCurrentHashRequest;
    using Res = GetCurrentHashResponse;

    public class GetCurrentHashCommand : IGetCurrentHashCommand
    {
        private readonly IRepositoryFactory repositoryFactory;

        public GetCurrentHashCommand(
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

            return from hash in repo.GetCurrentHash(req.ScriptName)
                   select CreateResponse(req, hash);
        }

        private static Res CreateResponse(Req req, string hash)
        {
            var res = new Res { Hash = hash };
            return Mapper.Map(req, res);
        }
    }
}
