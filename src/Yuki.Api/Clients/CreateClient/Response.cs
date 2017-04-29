namespace Yuki.Api.Clients.CreateClient
{
    using Newtonsoft.Json;

    public class Response
    {
        public Response(ClientData data)
        {
            this.Data = data;
        }

        [JsonProperty("data")]
        public ClientData Data
        {
            get;
            private set;
        }
    }
}