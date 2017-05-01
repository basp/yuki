namespace Yuki.Api.Tags
{
    using Newtonsoft.Json;

    public class TagData
    {
        [JsonProperty("id")]
        public int Id
        {
            get;
            set;
        }

        [JsonProperty("name")]
        public string Name
        {
            get;
            set;
        }
        
        [JsonProperty("wid")]
        public int Wid
        {
            get;
            set;
        }
    }
}