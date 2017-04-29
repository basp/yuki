namespace Yuki.Api.Clients.CreateClient
{
    using Newtonsoft.Json;

    public class Request
    {
        public Request(ClientData client)
        {
            this.Client = client;
        }

        [JsonProperty("client")]
        public ClientData Client
        {
            get;
            private set;
        }
    }
}