namespace Yuki.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.Contracts;
    using NLog;
    using Optional;
    using SmartFormat;

    using static Optional.Option;

    using Req = CreateDatabaseRequest;
    using Res = CreateDatabaseResponse;

    public class CreateDatabaseCommand : ICommand<Req, Res, Exception>
    {
        private static readonly string CreateDatabaseTemplate =
            $"{nameof(Yuki)}.Resources.CreateDatabase.sql";

        private readonly ILogger log = LogManager.GetCurrentClassLogger();

        private readonly ISession session;

        public CreateDatabaseCommand(ISession session)
        {
            Contract.Requires(session != null);

            this.session = session;
        }

        public Option<Res, Exception> Execute(Req req)
        {
            try
            {
                var asm = typeof(CreateDatabaseCommand).Assembly;
                var tmpl = asm.ReadEmbeddedString(CreateDatabaseTemplate);

                var cmdText = Smart.Format(tmpl, req);
                var res = (bool)this.session.ExecuteScalar(
                    cmdText,
                    new Dictionary<string, object>(),
                    CommandType.Text);

                var response = CreateResponse(res, req.Database, req.Server);
                return Some<Res, Exception>(response);
            }
            catch (Exception ex)
            {
                var msg = $"Could not create database '{req.Database}' on server '{req.Server}'.";
                var error = new Exception(msg, ex);
                return None<Res, Exception>(error);
            }
        }

        private static Res CreateResponse(bool created, string database, string server)
        {
            return new Res(server, database)
            {
                Created = created,
            };
        }
    }
}
