namespace Yuki.Api.Clients.CreateClient
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class Request
    {
        [JsonProperty("client")]
        public IDictionary<string, object> Client
        {
            get;
            private set;
        }
    }
}