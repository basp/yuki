namespace Yuki.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.Contracts;
    using Optional;
    using Optional.Linq;
    using Templates;

    using Req = CreateDatabaseRequest;
    using Res = CreateDatabaseResponse;

    public class CreateDatabaseCommand : ICreateDatabaseCommand
    {
        private readonly ISession session;

        public CreateDatabaseCommand(ISession session)
        {
            Contract.Requires(session != null);

            this.session = session;
        }

        public Option<Res, Exception> Execute(Req req)
        {
            return this.CreateDatabase(req)
                .Map(x => CreateResponse(req, x));
        }

        private static Res CreateResponse(Req req, bool created)
        {
            return new Res
            {
                Server = req.Server,
                Database = req.Database,
                Created = created,
            };
        }

        private Option<bool, Exception> CreateDatabase(Req req)
        {
            var tmpl = new CreateDatabaseTemplate(req.Database);
            return from cmdText in tmpl.Format()
                   let args = new Dictionary<string, object>()
                   from res in this.session.TryExecuteScalar<bool>(
                       cmdText,
                       args,
                       CommandType.Text)
                   select res;
        }
    }
}
