namespace Yuki.Api.TimeEntries.CreateTimeEntry
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class Request
    {
        [JsonIgnore]
        public int UserId
        {
            get;
            private set;
        }

        [JsonProperty("time_entry")]
        public IDictionary<string, object> TimeEntry
        {
            get;
            set;
        }

        public Request WithUserId(int userId)
        {
            this.UserId = userId;
            return this;
        }
    }
}