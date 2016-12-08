namespace Yuki
{
    using Newtonsoft.Json;

    public class Database
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "folder")]
        public string Folder { get; set; }
    }
}
