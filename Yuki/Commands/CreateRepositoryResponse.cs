namespace Yuki.Commands
{
    using System.Diagnostics.Contracts;
    using Newtonsoft.Json;

    public class CreateRepositoryResponse
    {
        public CreateRepositoryResponse(
            string database,
            string schema)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(database));
            Contract.Requires(!string.IsNullOrWhiteSpace(schema));

            this.Database = database;
            this.Schema = schema;
        }

        [JsonProperty(PropertyName = "database")]
        public string Database
        {
            get;
            private set;
        }

        [JsonProperty(PropertyName = "schema")]
        public string Schema
        {
            get;
            private set;
        }
    }
}
