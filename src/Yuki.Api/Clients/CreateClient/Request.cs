namespace Yuki.Api.Clients.CreateClient
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