namespace Yuki.Commands
{
    using System;
    using System.Diagnostics.Contracts;
    using Optional;

    using static Optional.Option;

    using Req = InsertScriptRunRequest;
    using Res = InsertScriptRunResponse;

    public class InsertScriptRunCommand : ICommand<Req, Res, Exception>
    {
        private readonly ISession session;
        private readonly IIdentityProvider identity;
        private readonly IHasher hasher;

        public InsertScriptRunCommand(
            ISession session,
            IIdentityProvider identity,
            IHasher hasher)
        {
            Contract.Requires(session != null);
            Contract.Requires(identity != null);
            Contract.Requires(hasher != null);

            this.session = session;
            this.identity = identity;
            this.hasher = hasher;
        }

        public Option<Res, Exception> Execute(Req request)
        {
            request.EnteredBy = this.identity
                .GetCurrent()
                .ValueOr(Environment.MachineName);

            var repo = new SqlRepository(this.session, request);
            var res = repo.InsertScriptRun(request);

            return res.Map(x => CreateResult(x, request))
                .MapException(x => (Exception)x);
        }

        private static Res CreateResult(int scriptRunId, Req request)
        {
            return new Res()
            {
                Server = request.Server,
                Database = request.Database,
                Schema = request.Schema,
                ScriptRunId = scriptRunId,
                EnteredBy = request.EnteredBy,
                Hash = request.Hash,
                IsOneTimeScript = request.IsOneTimeScript,
                ScriptName = request.ScriptName,
                VersionId = request.VersionId
            };
        }
    }
}
