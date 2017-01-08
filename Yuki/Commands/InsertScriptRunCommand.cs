namespace Yuki.Commands
{
    using System;
    using System.Data;
    using System.Diagnostics.Contracts;
    using AutoMapper;
    using Optional;
    using Optional.Linq;

    using Req = InsertScriptRunRequest;
    using Res = InsertScriptRunResponse;

    public class InsertScriptRunCommand : IInsertScriptRunCommand
    {
        private readonly IRepositoryFactory repositoryFactory;
        private readonly IIdentityProvider identityProvider;

        public InsertScriptRunCommand(
            IRepositoryFactory repositoryFactory,
            IIdentityProvider identityProvider)
        {
            Contract.Requires(repositoryFactory != null);
            Contract.Requires(identityProvider != null);

            this.repositoryFactory = repositoryFactory;
            this.identityProvider = identityProvider;
        }

        public Option<Res, Exception> Execute(Req req)
        {
            var repo = this.repositoryFactory.Create(
                req.RepositoryDatabase,
                req.RepositorySchema);

            var user = this.identityProvider.GetCurrent()
                .ValueOr(Environment.MachineName);

            var record = Mapper.Map(req, new ScriptRunRecord
            {
                EnteredBy = user,
            });

            return from id in repo.InsertScriptRun(record)
                   select CreateResponse(req, id);
        }

        private static Res CreateResponse(Req req, int scriptRunId)
        {
            return Mapper.Map(req, new Res
            {
                ScriptRunId = scriptRunId,
            });
        }
    }
}