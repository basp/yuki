namespace Yuki.Commands
{
    using System;
    using System.Data;
    using System.Diagnostics.Contracts;
    using AutoMapper;
    using Optional;
    using Optional.Linq;

    using Req = InsertScriptRunErrorRequest;
    using Res = InsertScriptRunErrorResponse;

    public class InsertScriptRunErrorCommand : IInsertScriptRunErrorCommand
    {
        private readonly IRepositoryFactory repositoryFactory;
        private readonly IIdentityProvider identityProvider;

        public InsertScriptRunErrorCommand(
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
            var user = this.identityProvider.GetCurrent()
                .ValueOr(Environment.MachineName);

            var repo = this.repositoryFactory.Create(
                req.RepositoryDatabase,
                req.RepositorySchema);

            var record = Mapper.Map(req, new ScriptRunErrorRecord
            {
                EnteredBy = user,
            });

            return from id in repo.InsetScriptRunError(record)
                   select CreateResponse(req, id);
        }

        private static Res CreateResponse(Req req, int scriptRunErrorId)
        {
            return Mapper.Map(req, new Res
            {
                ScriptRunErrorId = scriptRunErrorId,
            });
        }
    }
}
