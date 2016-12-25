namespace Yuki.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.Contracts;
    using NLog;
    using Optional;
    using Optional.Linq;
    using Templates;

    using static Optional.Option;

    using Req = InitializeRepositoryRequest;
    using Res = InitializeRepositoryResponse;

    public class InitializeRepositoryCommand : IInitializeRepositoryCommand
    {
        private readonly ILogger log = LogManager.GetCurrentClassLogger();

        private readonly ISession session;

        public InitializeRepositoryCommand(ISession session)
        {
            Contract.Requires(session != null);

            this.session = session;
        }

        public Option<Res, Exception> Execute(Req req)
        {
            var tmpl = new CreateRepositoryTemplate(
                req.RepositoryDatabase,
                req.RepositorySchema);

            return from sql in tmpl.Format()
                   let stmts = StatementSplitter.Split(sql)
                   from res in this.ExecuteStatements(stmts)
                   select CreateResponse(req);
        }

        private static Res CreateResponse(Req req)
        {
            return new Res
            {
                Server = req.Server,
                RepositoryDatabase = req.RepositoryDatabase,
                RepositorySchema = req.RepositorySchema,
            };
        }

        private Option<bool, Exception> ExecuteStatements(
            IEnumerable<string> stmts)
        {
            try
            {
                foreach (var s in stmts)
                {
                    var args = new Dictionary<string, object>();
                    var ct = CommandType.Text;
                    this.session.ExecuteNonQuery(s, args, ct);
                }

                return Some<bool, Exception>(true);
            }
            catch (Exception ex)
            {
                return None<bool, Exception>(ex);
            }
        }
    }
}
