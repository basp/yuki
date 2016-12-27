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

    using Req = HasScriptRunRequest;
    using Res = HasScriptRunResponse;

    public class HasScriptRunCommand : IHasScriptRunCommand
    {
        private readonly ILogger log = LogManager.GetCurrentClassLogger();

        private readonly ISession session;

        public HasScriptRunCommand(ISession session)
        {
            Contract.Requires(session != null);

            this.session = session;
        }

        public Option<Res, Exception> Execute(Req req)
        {
            var sp = FullyQualifiedObjectName(
                req.RepositoryDatabase,
                req.RepositorySchema,
                "HasScriptRunAlready");

            var args = new Dictionary<string, object>
            {
                ["ScriptName"] = req.ScriptName,
            };

            var hasScriptRunResponse = this.session.TryExecuteScalar<int>(
                sp,
                args,
                CommandType.StoredProcedure);

            return from count in hasScriptRunResponse
                   select CreateResponse(req, count > 0);
        }

        private static Res CreateResponse(Req req, bool hasRunAlready)
        {
            return new Res
            {
                ScriptName = req.ScriptName,
                HasRunAlready = hasRunAlready,
            };
        }
    }
}
