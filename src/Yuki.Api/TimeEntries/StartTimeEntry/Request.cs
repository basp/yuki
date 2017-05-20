namespace Yuki.Api.TimeEntries.StartTimeEntry
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class Request
    {
        public Request()
        {
            this.TimeEntry = new Dictionary<string, object>();
        }

        public Request(IDictionary<string,object> timeEntry)
        {
            this.TimeEntry = timeEntry;
        }

        [JsonIgnore]
        public int UserId
        {
            get;
            set;
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