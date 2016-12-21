namespace Yuki.Commands
{
    using Newtonsoft.Json;

    public class InsertScriptRunResponse : IRepositoryResponse
    {
        [JsonProperty(PropertyName = "server")]
        public string Server { get; set; }

        [JsonProperty(PropertyName = "database")]
        public string Database { get; set; }

        [JsonProperty(PropertyName = "schema")]
        public string Schema { get; set; }

        [JsonProperty(PropertyName = "scriptRunId")]
        public int ScriptRunId { get; set; }

        [JsonProperty(PropertyName = "enteredBy")]
        public string EnteredBy { get; set; }

        [JsonProperty(PropertyName = "versionId")]
        public int VersionId { get; set; }

        [JsonProperty(PropertyName = "scriptName")]
        public string ScriptName { get; set; }

        [JsonProperty(PropertyName = "hash")]
        public string Hash { get; set; }

        [JsonProperty(PropertyName = "isOneTimeScript")]
        public bool IsOneTimeScript { get; set; }

        [JsonIgnore]
        public string Sql { get; set; }
    }
}
