namespace Yuki.Commands
{
    using System.Diagnostics.Contracts;
    using Newtonsoft.Json;

    public class DatabaseResponse : IDatabaseResponse
    {
        public DatabaseResponse(string server, string database)
        {
            this.Server = server;
            this.Database = database;
        }

        [JsonProperty(PropertyName = "server")]
        public string Server
        {
            get;
            private set;
        }

        [JsonProperty(PropertyName = "database")]
        public string Database
        {
            get;
            private set;
        }
    }
}
