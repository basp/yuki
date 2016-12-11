namespace Yuki.Commands
{
    using System.Diagnostics.Contracts;
    using Optional;

    using Ex = SqlRepositoryException;
    using Req = CreateRepositoryRequest;
    using Res = CreateRepositoryResponse;

    public class CreateRepositoryCommand : ICommand<Req, Res, Ex>
    {
        private readonly ISession session;
        private readonly IIdentityProvider identity;

        public CreateRepositoryCommand(
            ISession session,
            IIdentityProvider identity)
        {
            Contract.Requires(session != null);
            Contract.Requires(identity != null);

            this.session = session;
            this.identity = identity;
        }

        public Option<Res, Ex> Execute(Req req)
        {
            var repository = new SqlRepository(
                this.session,
                this.identity,
                req);

            var res = repository.Initialize();
            return res.Map(x => new Res(req.Database, req.Schema));
        }
    }
}
