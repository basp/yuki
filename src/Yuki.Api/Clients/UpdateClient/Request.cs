namespace Yuki.Api.Clients.UpdateClient
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class Request
    {
        [JsonIgnore]
        public int ClientId
        {
            get;
            set;
        }

        [JsonProperty("client")]
        public IDictionary<string, object> Client
        {
            get;
            set;
        }
    }
}