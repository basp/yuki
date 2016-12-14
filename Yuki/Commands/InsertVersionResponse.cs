namespace Yuki.Commands
{
    using System;
    using Newtonsoft.Json;

    public class InsertVersionResponse : IRepositoryResponse
    {
        [JsonProperty(PropertyName = "server")]
        public string Server { get; set; }

        [JsonProperty(PropertyName = "database")]
        public string Database { get; set; }

        [JsonProperty(PropertyName = "schema")]
        public string Schema { get; set; }

        [JsonProperty(PropertyName = "versionId")]
        public int VersionId { get; set; }

        [JsonProperty(PropertyName = "versionName")]
        public string VersionName { get; set; }

        [JsonProperty(PropertyName = "respositoryPath")]
        public string RepositoryPath { get; set; }

        [JsonProperty(PropertyName = "enteredBy")]
        public string EnteredBy { get; set; }
    }
}
