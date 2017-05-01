namespace Yuki.Api.Clients.UpdateClient
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