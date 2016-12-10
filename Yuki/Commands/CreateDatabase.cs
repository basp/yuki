namespace Yuki.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.Contracts;
    using Optional;
    using SmartFormat;

    using Req = CreateDatabaseRequest;
    using Res = CreateDatabaseResponse;

    public class CreateDatabase : ICommand<Req, Res, Exception>
    {
        private static readonly string CreateDatabaseTemplate =
            $"{nameof(Yuki)}.Resources.CreateDatabase.sql";

        private readonly ISession session;

        public CreateDatabase(ISession session)
        {
            Contract.Requires(session != null);

            this.session = session;
        }

        public Option<Res, Exception> Execute(Req request)
        {
            try
            {
                var asm = typeof(CreateDatabase).Assembly;
                var tmpl = asm.ReadEmbeddedString(CreateDatabaseTemplate);

                var cmdText = Smart.Format(tmpl, request);
                var res = this.session.ExecuteNonQuery(
                    cmdText,
                    new Dictionary<string, object>(),
                    CommandType.Text);

                return Option.Some<Res, Exception>(Res.Created);
            }
            catch (Exception ex)
            {
                var msg = $"Could not create database '{request.Database}' on server '{request.Server}'.";
                var error = new Exception(msg, ex);
                return Option.None<Res, Exception>(error);
            }
        }
    }
}
