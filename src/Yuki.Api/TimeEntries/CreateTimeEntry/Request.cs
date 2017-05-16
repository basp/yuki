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
            set;
        }

        [JsonProperty("time_entry")]
        public IDictionary<string, object> TimeEntry
        {
            get;
            set;
        }
    }
}