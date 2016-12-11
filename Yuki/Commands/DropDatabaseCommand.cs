namespace Yuki.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.Contracts;
    using Optional;
    using SmartFormat;

    using Req = DropDatabaseRequest;
    using Res = DropDatabaseResponse;

    public class DropDatabaseCommand : ICommand<Req, Res, Exception>
    {
        private static readonly string ResourceName =
            $"{nameof(Yuki)}.Resources.DropDatabase.sql";

        private readonly ISession session;

        public DropDatabaseCommand(ISession session)
        {
            Contract.Requires(session != null);

            this.session = session;
        }

        public Option<Res, Exception> Execute(Req req)
        {
            try
            {
                var asm = typeof(DropDatabaseCommand).Assembly;
                var tmpl = asm.ReadEmbeddedString(ResourceName);
                var cmdText = Smart.Format(tmpl, req);

                this.session.ExecuteNonQuery(
                    cmdText,
                    new Dictionary<string, object>(),
                    CommandType.Text);

                return Option.Some<Res, Exception>(
                    new Res(req.Server, req.Database));
            }
            catch (Exception ex)
            {
                var msg = $"Could not drop database '{req.Database}' on server '{req.Server}'.";
                var error = new Exception(msg, ex);
                return Option.None<Res, Exception>(error);
            }
        }
    }
}
