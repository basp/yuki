namespace Yuki.Api.Projects
{
    using Newtonsoft.Json;

    public class ProjectData
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

        [JsonProperty("cid", NullValueHandling = NullValueHandling.Ignore)]
        public int? Cid
        {
            get;
            set;
        }

        [JsonProperty("active")]
        public bool Active
        {
            get;
            set;
        }

        [JsonProperty("is_private")]
        private bool IsPrivate
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