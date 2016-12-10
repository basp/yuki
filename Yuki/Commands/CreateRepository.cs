namespace Yuki.Commands
{
    using System.Diagnostics.Contracts;
    using Optional;

    using Ex = SqlRepositoryException;
    using Req = CreateRepositoryRequest;
    using Res = CreateRepositoryResponse;

    public class CreateRepository : ICommand<Req, Res, Ex>
    {
        private readonly ISession session;

        public CreateRepository(ISession session)
        {
            Contract.Requires(session != null);

            this.session = session;
        }

        public Option<Res, Ex> Execute(Req request)
        {
            var repository = new SqlRepository(this.session);
            var res = repository.Initialize(
                request.Database,
                request.Schema);

            return res.Map(x => x ? Res.Created : Res.None);
        }
    }
}
