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

    public class DropDatabase : ICommand<Req, Res, Exception>
    {
        private static readonly string ResourceName =
            $"{nameof(Yuki)}.Resources.DropDatabase.sql";

        private readonly ISession session;

        public DropDatabase(ISession session)
        {
            Contract.Requires(session != null);

            this.session = session;
        }

        public Option<Res, Exception> Execute(Req request)
        {
            try
            {
                var asm = typeof(DropDatabase).Assembly;
                var tmpl = asm.ReadEmbeddedString(ResourceName);
                var cmdText = Smart.Format(tmpl, request);

                this.session.ExecuteNonQuery(
                    cmdText,
                    new Dictionary<string, object>(),
                    CommandType.Text);

                return Option.Some<Res, Exception>(Res.Dropped);
            }
            catch (Exception ex)
            {
                var msg = $"Could not drop database '{request.Database}' on server '{request.Server}'.";
                var error = new Exception(msg, ex);
                return Option.None<Res, Exception>(error);
            }
        }
    }
}
