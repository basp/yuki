namespace Yuki.Commands
{
    using Newtonsoft.Json;

    public class SetupDatabaseResponse
    {
        [JsonProperty(PropertyName = "server")]
        public string Server { get; set; }

        [JsonProperty(PropertyName = "database")]
        public string Database { get; set; }

        [JsonProperty(PropertyName = "folder")]
        public string Folder { get; set; }

        [JsonProperty(PropertyName = "backup")]
        public string Backup { get; set; }
    }
}
