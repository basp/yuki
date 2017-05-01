namespace Yuki.Api.Groups.DeleteGroup
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
    }
}