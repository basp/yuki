namespace Yuki.Api.Groups.CreateGroup
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class Request
    {
        [JsonProperty("group")]
        public IDictionary<string, object> Group
        {
            get;
            set;
        }
    }
}