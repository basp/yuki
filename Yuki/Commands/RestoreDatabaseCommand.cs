namespace Yuki.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.Contracts;
    using NLog;
    using Optional;
    using Templates;

    using Req = RestoreDatabaseRequest;
    using Res = RestoreDatabaseResponse;

    public class RestoreDatabaseCommand : IRestoreDatabaseCommand
    {
        private readonly ILogger log = LogManager.GetCurrentClassLogger();

        private readonly ISession session;

        public RestoreDatabaseCommand(ISession session)
        {
            Contract.Requires(session != null);

            this.session = session;
        }

        public Option<Res, Exception> Execute(Req req)
        {
            return this.RestoreDatabase(req)
                .Map(x => CreateResponse(req));
        }

        private static Res CreateResponse(Req req)
        {
            return new Res
            {
                Restored = true,
                Backup = req.Backup,
            };
        }

        private Option<bool, Exception> RestoreDatabase(Req req)
        {
            var tmpl = new RestoreDatabaseTemplate(req.Database);
            var args = new Dictionary<string, object>
            {
                ["Backup"] = req.Backup,
            };

            this.log.Info(
                "Restoring [{0}] on {1} using backup {2}",
                req.Database,
                this.session,
                req.Backup);

            return tmpl.Format()
                .FlatMap(cmdText => this.session.TryExecuteNonQuery(
                    cmdText,
                    args,
                    CommandType.Text))
                .Map(x => true);
        }
    }
}
