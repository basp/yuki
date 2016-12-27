namespace Yuki.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.Contracts;
    using Optional;
    using Optional.Linq;

    using static Utils;

    using Req = InsertVersionRequest;
    using Res = InsertVersionResponse;

    public class InsertVersionCommand : IInsertVersionCommand
    {
        private readonly ISession session;
        private readonly IIdentityProvider identityProvider;

        public InsertVersionCommand(
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
                "InsertVersion");

            var user = this.identityProvider.GetCurrent()
                .ValueOr(Environment.MachineName);

            var args = new Dictionary<string, object>
            {
                ["VersionName"] = req.RepositoryVersion,
                ["RepositoryPath"] = req.RepositoryPath,
                ["EnteredBy"] = user,
            };

            var scalar = this.session.TryExecuteScalar<int>(
                sp,
                args,
                CommandType.StoredProcedure);

            return from id in scalar select CreateResponse(req, id);
        }

        private static Res CreateResponse(Req req, int versionId)
        {
            return new Res
            {
                VersionId = versionId,
            };
        }
    }
}
