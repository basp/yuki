namespace Yuki.Commands
{
    using System;
    using System.Data;
    using System.Diagnostics.Contracts;
    using AutoMapper;
    using Optional;
    using Optional.Linq;

    using Req = HasScriptRunRequest;
    using Res = HasScriptRunResponse;

    public class HasScriptRunCommand : IHasScriptRunCommand
    {
        private readonly IRepositoryFactory repositoryFactory;

        public HasScriptRunCommand(
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

            return from hasRunAlready in repo.HasScriptRun(req.ScriptName)
                   select CreateResponse(req, hasRunAlready);
        }

        private static Res CreateResponse(Req req, bool hasRunAlready)
        {
            var res = new Res
            {
                HasRunAlready = hasRunAlready,
            };

            return Mapper.Map(req, res);
        }
    }
}
