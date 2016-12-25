namespace Yuki.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.Contracts;
    using Optional;
    using Optional.Linq;

    using static Utils;

    using Req = GetVersionRequest;
    using Res = GetVersionResponse;

    public class GetVersionCommand
        : IGetVersionCommand
    {
        private readonly ISession session;

        public GetVersionCommand(ISession session)
        {
            Contract.Requires(session != null);

            this.session = session;
        }

        public Option<Res, Exception> Execute(Req req)
        {
            var sp = FullyQualifiedObjectName(
                req.RepositoryDatabase,
                req.RepositorySchema,
                "GetVersion");

            var args = new Dictionary<string, object>
            {
                ["RepositoryPath"] = req.RepositoryPath,
            };

            var versionResponse = this.session.TryExecuteScalar<string>(
                sp,
                args,
                CommandType.StoredProcedure);

            return from v in versionResponse
                   select CreateResponse(req, v);
        }

        private static Res CreateResponse(
            Req req,
            string versionName)
        {
            return new Res
            {
                VersionName = versionName,
            };
        }
    }
}