namespace Yuki.Api.Clients.GetClientDetails
{
    public class Response
    {
        public Response(ClientData data)
        {
            this.Data = data;
        }

        public ClientData Data
        {
            get;
            private set;
        }
    }
}