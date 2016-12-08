namespace Yuki.Cmd
{
    using Newtonsoft.Json;

    public class Config
    {
        [JsonProperty(PropertyName = "folders")]
        public Folder[] Folders { get; set; }
    }
}
