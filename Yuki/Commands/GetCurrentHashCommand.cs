namespace Yuki.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.Contracts;
    using NLog;
    using Optional;
    using Optional.Linq;

    using static Utils;

    using Req = GetCurrentHashRequest;
    using Res = GetCurrentHashResponse;

    public class GetCurrentHashCommand : IGetCurrentHashCommand
    {
        private readonly ILogger log = LogManager.GetCurrentClassLogger();

        private readonly ISession session;

        public GetCurrentHashCommand(ISession session)
        {
            Contract.Requires(session != null);

            this.session = session;
        }

        public Option<Res, Exception> Execute(Req req)
        {
            var sp = FullyQualifiedObjectName(
                req.RepositoryDatabase,
                req.RepositorySchema,
                "GetCurrentScriptHash");

            var args = new Dictionary<string, object>
            {
                ["ScriptName"] = req.ScriptName,
            };

            var getHashResult = this.session.TryExecuteScalar<string>(
                sp,
                args,
                CommandType.StoredProcedure);

            return from hash in getHashResult select CreateResponse(req, hash);
        }

        private static Res CreateResponse(Req req, string hash)
        {
            return new Res
            {
                ScriptName = req.ScriptName,
                Hash = hash,
            };
        }
    }
}
