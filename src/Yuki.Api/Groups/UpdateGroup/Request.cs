namespace Yuki.Api.Groups.UpdateGroup
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class Request
    {
        [JsonIgnore]
        public int GroupId
        {
            get;
            set;
        }

        [JsonProperty("group")]
        public IDictionary<string, object> Group
        {
            get;
            set;
        }
    }
}