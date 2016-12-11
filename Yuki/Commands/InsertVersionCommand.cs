namespace Yuki.Commands
{
    using System;
    using System.Diagnostics.Contracts;
    using Optional;

    using static Optional.Option;

    using Req = InsertVersionRequest;
    using Res = InsertVersionResponse;

    public class InsertVersionCommand : ICommand<Req, Res, Exception>
    {
        private readonly ISession session;
        private readonly IIdentityProvider identity;

        public InsertVersionCommand(ISession session, IIdentityProvider identity)
        {
            Contract.Requires(session != null);
            Contract.Requires(identity != null);

            this.session = session;
            this.identity = identity;
        }

        public Option<Res, Exception> Execute(Req request)
        {
            try
            {
                request.EnteredBy = this.identity.GetCurrent()
                    .ValueOr(Environment.MachineName);

                var repository = new SqlRepository(this.session, request);
                var result = repository.InsertVersion(request);
                return result.Map(x => CreateResponse(x, request))
                    .MapException(x => (Exception)x);
            }
            catch (Exception ex)
            {
                var msg = $"Could not insert version.";
                var error = new Exception(msg, ex);
                return None<Res, Exception>(error);
            }
        }

        private static Res CreateResponse(int versionId, Req request)
        {
            return new Res()
            {
                VersionId = versionId,
                VersionName = request.VersionName,
                RepositoryPath = request.RepositoryPath,
                EnteredBy = request.EnteredBy
            };
        }
    }
}
