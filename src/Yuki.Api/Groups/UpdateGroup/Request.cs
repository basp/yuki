namespace Yuki.Api.Groups.UpdateGroup
{
    using Newtonsoft.Json;

    public class Request
    {
        [JsonIgnore]
        public int GroupId
        {
            get;
            set;
        }

        [JsonProperty("group")]
        public GroupData Group
        {
            get;
            set;
        }
    }
}