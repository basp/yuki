namespace Yuki.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.Contracts;
    using Optional;
    using Optional.Linq;

    using static Utils;

    using Req = InsertScriptRunErrorRequest;
    using Res = InsertScriptRunErrorResponse;

    public class InsertScriptRunErrorCommand : IInsertScriptRunErrorCommand
    {
        private readonly ISession session;
        private readonly IIdentityProvider identityProvider;

        public InsertScriptRunErrorCommand(
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
                "InsertScriptRunError");

            var user = this.identityProvider.GetCurrent()
                .ValueOr(Environment.MachineName);

            var args = new Dictionary<string, object>
            {
                ["RepositoryPath"] = req.RepositoryPath,
                ["ScriptName"] = req.ScriptName,
                ["VersionName"] = req.VersionName,
                ["TextOfScript"] = req.Sql,
                ["ErroneousPart"] = req.SqlErrorPart,
                ["ErrorMessage"] = req.ErrorMessage,
                ["EnteredBy"] = user,
            };

            var scalar = this.session.TryExecuteScalar<int>(
                sp,
                args,
                CommandType.StoredProcedure);

            return from id in scalar select CreateResponse(req, id);
        }

        private static Res CreateResponse(Req req, int scriptRunErrorId)
        {
            return new Res
            {
                ScriptRunErrorId = scriptRunErrorId,
                VersionName = req.VersionName,
                ScriptName = req.ScriptName,
                SqlErrorPart = req.SqlErrorPart,
                ErrorMessage = req.ErrorMessage,
            };
        }
    }
}
