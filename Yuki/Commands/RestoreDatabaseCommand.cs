namespace Yuki.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.Contracts;
    using Optional;
    using Optional.Linq;
    using Serilog;
    using Templates;

    using Req = RestoreDatabaseRequest;
    using Res = RestoreDatabaseResponse;

    public class RestoreDatabaseCommand : IRestoreDatabaseCommand
    {
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

            Log.Information(
                "Restoring {Database} on {Server} using backup {BackupFile}",
                req.Database,
                this.session,
                req.Backup);

            return from cmdText in tmpl.Format()
                   let args = new Dictionary<string, object>
                   {
                       ["Backup"] = req.Backup,
                   }
                   from res in this.session.TryExecuteNonQuery(
                       cmdText,
                       args,
                       CommandType.Text)
                   select true;
        }
    }
}