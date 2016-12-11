namespace Yuki.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.Contracts;
    using Dapper;
    using Optional;

    using Res = QueryResponse;
    using Req = QueryRequest;

    public class QueryCommand : ICommand<Req, Res, Exception>
    {
        private readonly ISession session;

        public QueryCommand(ISession session)
        {
            Contract.Requires(session != null);

            this.session = session;
        }

        public Option<Res, Exception> Execute(Req request)
        {
            try
            {
                var @default = new Dictionary<string, object>();
                var @params = request.Args.ValueOr(@default);

                var result = this.session
                    .Connection
                    .Query(request.Sql, @params);

                var res = new Res(
                    request.Server,
                    Option.Some<IEnumerable<dynamic>, Exception>(result));

                return Option.Some<Res, Exception>(res);
            }
            catch (Exception ex)
            {
                return Option.None<Res, Exception>(ex);
            }
        }
    }
}
