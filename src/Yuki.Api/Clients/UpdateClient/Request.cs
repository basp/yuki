namespace Yuki.Api.Clients.UpdateClient
{
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
        public ClientData Client
        {
            get;
            set;
        }
    }
}