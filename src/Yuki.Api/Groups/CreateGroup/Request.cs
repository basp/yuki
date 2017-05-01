namespace Yuki.Api.Groups.CreateGroup
{
    using Newtonsoft.Json;

    public class Request
    {
        [JsonProperty("group")]
        public GroupData Group
        {
            get;
            set;
        }
    }
}