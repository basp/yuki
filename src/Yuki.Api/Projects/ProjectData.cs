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

        [JsonProperty("cid")]
        public int Cid
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

        [JsonProperty("at")]
        public string At
        {
            get;
            set;
        }
    }
}