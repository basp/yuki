namespace Yuki.Api.Groups.DeleteGroup
{
    using Newtonsoft.Json;

    public class Response
    {
        public Response(GroupData data)
        {
            this.Data = data;
        }

        [JsonProperty("data")]
        public GroupData Data
        {
            get;
            private set;
        }
    }
}