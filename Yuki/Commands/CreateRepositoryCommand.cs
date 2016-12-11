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

        public CreateRepositoryCommand(ISession session)
        {
            Contract.Requires(session != null);

            this.session = session;
        }

        public Option<Res, Ex> Execute(Req req)
        {
            var repository = new SqlRepository(
                this.session,
                req);

            var res = repository.Initialize();
            return res.Map(x => new Res(req.Database, req.Schema));
        }
    }
}
