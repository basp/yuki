namespace Yuki.Api.Groups
{
    using Newtonsoft.Json;

    public class GroupData
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

        [JsonProperty("at")]
        public string At
        {
            get;
            set;
        }
    }
}