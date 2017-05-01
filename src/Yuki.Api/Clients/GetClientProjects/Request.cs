namespace Yuki.Api.Clients.GetClientProjects
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