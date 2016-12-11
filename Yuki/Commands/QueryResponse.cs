namespace Yuki.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using Optional;

    public class QueryResponse
    {
        public QueryResponse(string server, Option<IEnumerable<dynamic>, Exception> result)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(server));

            this.Server = server;
            this.Result = result;
        }

        public string Server
        {
            get;
            private set;
        }

        public Option<IEnumerable<dynamic>, Exception> Result
        {
            get;
            private set;
        }
    }
}