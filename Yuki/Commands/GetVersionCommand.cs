namespace Yuki.Commands
{
    using System;
    using System.Diagnostics.Contracts;
    using Optional;

    using Req = GetVersionRequest;
    using Res = GetVersionResponse;

    public class GetVersionCommand : ICommand<Req, Res, Exception>
    {
        private readonly ISession session;

        public GetVersionCommand(ISession session)
        {
            Contract.Requires(session != null);

            this.session = session;
        }

        public Option<Res, Exception> Execute(Req request)
        {
            var repository = new SqlRepository(this.session, request);
            var res = repository.GetVersion(request.RepositoryPath);
            return res
                .Map(x => CreateResult(x, request))
                .MapException(x => (Exception)x);
        }

        private static Res CreateResult(string versionName, Req request)
        {
            return new Res()
            {
                Server = request.Server,
                Database = request.RepositoryDatabase,
                Schema = request.RepositorySchema,
                RepositoryPath = request.RepositoryPath,
                VersionName = versionName,
            };
        }
    }
}
