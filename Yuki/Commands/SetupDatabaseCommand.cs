namespace Yuki.Commands
{
    using System;
    using System.Diagnostics.Contracts;
    using NLog;
    using Optional;

    using static Optional.Option;

    using Req = SetupDatabaseRequest;
    using Res = SetupDatabaseResponse;

    public class SetupDatabaseCommand : ICommand<Req, Res, Exception>
    {
        private readonly ILogger log = LogManager.GetCurrentClassLogger();
        private readonly ISession session;
        private readonly ICommand<CreateDatabaseRequest, CreateDatabaseResponse, Exception> createDatabaseCommand;

        public SetupDatabaseCommand(
            ISession session,
            ICommand<CreateDatabaseRequest, CreateDatabaseResponse, Exception> createDatabaseCommand)
        {
            Contract.Requires(session != null);
            Contract.Requires(createDatabaseCommand != null);

            this.session = session;
            this.createDatabaseCommand = createDatabaseCommand;
        }

        public Option<Res, Exception> Execute(Req request)
        {
            this.log.Info($"Creating database [{request.Database}] on server {request.Server} if it does not exist");

            var createDatabaseResult = this.CreateDatabaseCommand(request);
            if (!createDatabaseResult.HasValue)
            {
                return createDatabaseResult.Map(x => Res.Ok);
            }

            createDatabaseResult.MatchSome(x =>
            {
                var msg = x.Created
                    ? $"Created database [{request.Database}]"
                    : $"Database [{request.Database}] already exists";

                this.log.Info(msg);
            });

            return Some<Res, Exception>(Res.Ok);
        }

        private Option<CreateDatabaseResponse, Exception> CreateDatabaseCommand(Req request)
        {
            var createDatabaseRequest = new CreateDatabaseRequest()
            {
                Database = request.Database,
                Server = request.Server,
            };

            return this.createDatabaseCommand.Execute(createDatabaseRequest);
        }
    }
}
