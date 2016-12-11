namespace Yuki.Commands
{
    using System.Diagnostics.Contracts;
    using Optional;

    public class QueryFirstResponse
    {
        public QueryFirstResponse(string server, Option<dynamic> result)
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

        public Option<dynamic> Result
        {
            get;
            private set;
        }
    }
}
