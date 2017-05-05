namespace Yuki.Api.TimeEntries.CreateTimeEntry
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

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