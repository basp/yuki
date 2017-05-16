namespace Yuki.Api.Clients.GetClientDetails
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