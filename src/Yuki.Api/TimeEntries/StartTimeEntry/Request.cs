namespace Yuki.Api.TimeEntries.StartTimeEntry
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class Request
    {
        [JsonProperty("time_entry")]
        public IDictionary<string, object> TimeEntry
        {
            get;
            set;
        }
    }
}