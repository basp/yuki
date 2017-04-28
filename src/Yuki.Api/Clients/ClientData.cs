namespace Yuki.Api.Clients
{
    using Newtonsoft.Json;

    public class ClientData
    {
        [JsonProperty("id")]
        public int Id
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

        [JsonProperty("name")]
        public string Name
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

        [JsonProperty("notes")]
        public string Notes
        {
            get;
            set;
        }
    }
}