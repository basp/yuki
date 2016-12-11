namespace Yuki.Commands
{
    using System;
    using System.Diagnostics.Contracts;
    using Dapper;
    using Optional;

    using Req = QueryFirstRequest;
    using Res = QueryFirstResponse;

    public class QueryFirstCommand : ICommand<Req, Res, Exception>
    {
        private readonly ISession session;

        public QueryFirstCommand(ISession session)
        {
            Contract.Requires(session != null);

            this.session = session;
        }

        public Option<Res, Exception> Execute(Req request)
        {
            try
            {
                var result = this.session.Connection.QueryFirst(request.Sql);
                var res = new Res(request.Server, Option.Some<dynamic>(result));
                return Option.Some<Res, Exception>(res);
            }
            catch (Exception ex)
            {
                return Option.None<Res, Exception>(ex);
            }
        }
    }
}
