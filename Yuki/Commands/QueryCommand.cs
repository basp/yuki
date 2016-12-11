namespace Yuki.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using Dapper;
    using Optional;

    using Req = QueryRequest;
    using Res = QueryResponse;

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
                var result = this.session
                    .Connection
                    .Query(request.Sql);

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
