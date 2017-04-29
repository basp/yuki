namespace Yuki.Api.Clients.GetClientDetails
{
    public class Request
    {
        public Request(int clientId)
        {
            this.ClientId = clientId;
        }

        public int ClientId
        {
            get;
            private set;
        }
    }
}