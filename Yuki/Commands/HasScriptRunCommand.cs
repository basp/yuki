namespace Yuki.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.Contracts;
    using Optional;
    using Optional.Linq;

    using static Utils;

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
            return new Res
            {
                ScriptName = req.ScriptName,
                HasRunAlready = hasRunAlready,
            };
        }
    }
}
