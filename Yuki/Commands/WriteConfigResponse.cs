namespace Yuki.Commands
{
    using Newtonsoft.Json;

    public class WriteConfigResponse
    {
        [JsonProperty(PropertyName = "configFile")]
        public string ConfigFile { get; set; }

        [JsonProperty(PropertyName = "numberOfBytes")]
        public int NumberOfBytes { get; set; }
    }
}
