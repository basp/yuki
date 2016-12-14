namespace Yuki.Commands
{
    using Newtonsoft.Json;

    public class VersionResponse
    {
        [JsonProperty(PropertyName = "server")]
        public string Server { get; set; }

        [JsonProperty(PropertyName = "repositoryDatabase")]
        public string RepositoryDatabase { get; set; }

        [JsonProperty(PropertyName = "repositorySchema")]
        public string RepositorySchema { get; set; }

        [JsonProperty(PropertyName = "previousVersion")]
        public string PreviousVersion { get; set; }

        [JsonProperty(PropertyName = "currentVersion")]
        public string CurrentVersion { get; set; }

        [JsonProperty(PropertyName = "repositoryPath")]
        public string RepositoryPath { get; set; }

        [JsonProperty(PropertyName = "versionFile")]
        public string VersionFile { get; set; }

        [JsonProperty(PropertyName = "versionId")]
        public int VersionId { get; set; }
    }
}
