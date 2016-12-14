namespace Yuki.Commands
{
    using System;
    using System.Diagnostics.Contracts;
    using NLog;
    using Optional;

    using Req = CreateRepositoryRequest;
    using Res = CreateRepositoryResponse;

    public class CreateRepositoryCommand : ICommand<Req, Res, Exception>
    {
        private readonly ILogger log = LogManager.GetCurrentClassLogger();

        private readonly ISession session;

        public CreateRepositoryCommand(ISession session)
        {
            Contract.Requires(session != null);

            this.session = session;
        }

        public Option<Res, Exception> Execute(Req req)
        {
            this.log.Info($"Initializing repository in [{req.RepositoryDatabase}].[{req.RepositorySchema}]");

            var repository = new SqlRepository(
                this.session,
                req);

            var res = repository.Initialize();
            return res.Map(x => new Res(req.RepositoryDatabase, req.RepositorySchema))
                .MapException(x => new Exception($"Could not create repository.", x));
        }
    }
}
