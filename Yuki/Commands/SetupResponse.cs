namespace Yuki.Commands
{
    using Newtonsoft.Json;

    public class SetupResponse
    {
        [JsonProperty(PropertyName = "server")]
        public string Server { get; set; }

        [JsonProperty(PropertyName = "databaseFolder")]
        public string DatabaseFolder { get; set; }

        [JsonProperty(PropertyName = "repositoryDatabase")]
        public string RepositoryDatabase { get; set; }

        [JsonProperty(PropertyName = "repositorySchema")]

        public string RepositorySchema { get; set; }
    }
}
