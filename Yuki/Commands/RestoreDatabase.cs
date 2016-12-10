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

    public class RestoreDatabase : ICommand<Req, Res, Exception>
    {
        private static readonly string RestoreDatabaseTemplate =
            $"{nameof(Yuki)}.Resources.RestoreDatabase.sql";

        private readonly ISession session;

        public RestoreDatabase(ISession session)
        {
            Contract.Requires(session != null);

            this.session = session;
        }

        public Option<Res, Exception> Execute(Req request)
        {
            try
            {
                var asm = typeof(RestoreDatabase).Assembly;
                var tmpl = asm.ReadEmbeddedString(RestoreDatabaseTemplate);

                var cmdText = Smart.Format(tmpl, request);
                var args = new Dictionary<string, object>()
                {
                    ["Backup"] = request.Backup
                };

                var res = this.session.ExecuteNonQuery(
                    cmdText,
                    args,
                    CommandType.Text);

                return Option.Some<Res, Exception>(Res.Restored);
            }
            catch (Exception ex)
            {
                var msg = $"Could not restore database '{request.Database}' from '{request.Backup}' on server '{request.Server}'.";
                var res = new Exception(msg, ex);
                return Option.None<Res, Exception>(res);
            }
        }
    }
}
