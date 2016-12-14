namespace Yuki.Commands
{
    using System;
    using System.Diagnostics.Contracts;
    using Optional;

    using Req = CreateRepositoryRequest;
    using Res = CreateRepositoryResponse;

    public class CreateRepositoryCommand : ICommand<Req, Res, Exception>
    {
        private readonly ISession session;

        public CreateRepositoryCommand(ISession session)
        {
            Contract.Requires(session != null);

            this.session = session;
        }

        public Option<Res, Exception> Execute(Req req)
        {
            var repository = new SqlRepository(
                this.session,
                req);

            var res = repository.Initialize();
            return res.Map(x => new Res(req.Database, req.Schema))
                .MapException(x => new Exception($"Could not create repository.", x));
        }
    }
}
