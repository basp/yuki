namespace Yuki.Commands
{
    using Newtonsoft.Json;

    public class ReadFileResponse
    {
        [JsonProperty(PropertyName = "path")]
        public string Path { get; set; }

        [JsonProperty(PropertyName = "fileName")]
        public string FileName { get; set; }

        [JsonProperty(PropertyName = "contents")]
        public string Contents { get; set; }

        [JsonProperty(PropertyName = "hash")]
        public string Hash { get; set; }
    }
}
