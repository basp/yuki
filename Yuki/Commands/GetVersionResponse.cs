namespace Yuki.Commands
{
    using Newtonsoft.Json;

    public class GetVersionResponse : IRepositoryResponse
    {
        [JsonProperty(PropertyName = "server")]
        public string Server { get; set; }

        [JsonProperty(PropertyName = "database")]
        public string Database { get; set; }

        [JsonProperty(PropertyName = "schema")]
        public string Schema { get; set; }

        [JsonProperty(PropertyName = "repositoryPath")]
        public string RepositoryPath { get; set; }

        [JsonProperty(PropertyName = "versionName")]
        public string VersionName { get; set; }
    }
}
