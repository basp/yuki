namespace Yuki.Api.Clients.DeleteClient
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
    }
}