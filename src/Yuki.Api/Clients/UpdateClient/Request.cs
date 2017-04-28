namespace Yuki.Api.Clients.UpdateClient
{
    using Newtonsoft.Json;

    public class Request
    {
        [JsonProperty("client")]
        public ClientData Client
        {
            get;
            set;
        }
    }
}