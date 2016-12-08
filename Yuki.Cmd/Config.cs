namespace Yuki
{
    using Newtonsoft.Json;

    public class Config
    {
        [JsonProperty(PropertyName = "folders")]
        public Folder[] Folders { get; set; }
    }
}
