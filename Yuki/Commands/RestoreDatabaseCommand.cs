namespace Yuki.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.Contracts;
    using Optional;
    using SmartFormat;

    using Req = RestoreDatabaseRequest;
    using Res = RestoreDatabaseResponse;

    public class RestoreDatabaseCommand : ICommand<Req, Res, Exception>
    {
        private static readonly string RestoreDatabaseTemplate =
            $"{nameof(Yuki)}.Resources.RestoreDatabase.sql";

        private readonly ISession session;

        public RestoreDatabaseCommand(ISession session)
        {
            Contract.Requires(session != null);

            this.session = session;
        }

        public Option<Res, Exception> Execute(Req req)
        {
            try
            {
                var asm = typeof(RestoreDatabaseCommand).Assembly;
                var tmpl = asm.ReadEmbeddedString(RestoreDatabaseTemplate);

                var cmdText = Smart.Format(tmpl, req);
                var args = new Dictionary<string, object>()
                {
                    ["Backup"] = req.Backup,
                };

                var res = this.session.ExecuteNonQuery(
                    cmdText,
                    args,
                    CommandType.Text);

                return Option.Some<Res, Exception>(
                    new Res(req.Server, req.Database));
            }
            catch (Exception ex)
            {
                var msg = $"Could not restore database '{req.Database}' from '{req.Backup}' on server '{req.Server}'.";
                var res = new Exception(msg, ex);
                return Option.None<Res, Exception>(res);
            }
        }
    }
}
