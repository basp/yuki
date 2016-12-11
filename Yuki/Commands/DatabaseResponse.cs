namespace Yuki.Commands
{
    using System.Diagnostics.Contracts;

    public class DatabaseResponse : IDatabaseResponse
    {
        public DatabaseResponse(string server, string database)
        {
            this.Server = server;
            this.Database = database;
        }

        public string Server
        {
            get;
            private set;
        }

        public string Database
        {
            get;
            private set;
        }
    }
}
