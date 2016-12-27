namespace Yuki.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.Contracts;
    using Optional;
    using Optional.Linq;

    using static Utils;

    using Req = InsertScriptRunRequest;
    using Res = InsertScriptRunResponse;

    public class InsertScriptRunCommand : IInsertScriptRunCommand
    {
        private readonly ISession session;
        private readonly IIdentityProvider identityProvider;

        public InsertScriptRunCommand(
            ISession session,
            IIdentityProvider identityProvider)
        {
            Contract.Requires(session != null);
            Contract.Requires(identityProvider != null);

            this.session = session;
            this.identityProvider = identityProvider;
        }

        public Option<Res, Exception> Execute(Req req)
        {
            var sp = FullyQualifiedObjectName(
                req.RepositoryDatabase,
                req.RepositorySchema,
                "InsertScriptRun");

            var user = this.identityProvider.GetCurrent()
                .ValueOr(Environment.MachineName);

            var args = new Dictionary<string, object>
            {
                ["VersionId"] = req.VersionId,
                ["ScriptName"] = req.ScriptName,
                ["TextOfScript"] = req.Sql,
                ["TextHash"] = req.Hash,
                ["OneTimeScript"] = req.IsOneTimeScript,
                ["EnteredBy"] = user,
            };

            var scalar = this.session.TryExecuteScalar<int>(
                sp,
                args,
                CommandType.StoredProcedure);

            return from id in scalar select CreateResponse(req, id);
        }

        private static Res CreateResponse(Req req, int scriptRunId)
        {
            return new Res
            {
                ScriptName = req.ScriptName,
                VersionId = req.VersionId,
                ScriptRunId = scriptRunId,
            };
        }
    }
}