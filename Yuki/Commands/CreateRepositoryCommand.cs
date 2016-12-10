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

        public Option<Res, Ex> Execute(Req request)
        {
            var repository = new SqlRepository(
                this.session,
                this.identity);

            var res = repository.Initialize(
                request.Database,
                request.Schema);

            return res.Map(x => x ? Res.Created : Res.None);
        }
    }
}
