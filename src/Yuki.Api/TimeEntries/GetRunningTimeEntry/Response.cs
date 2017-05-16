namespace Yuki.Api.TimeEntries.GetRunningTimeEntry
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class Response
    {
        [JsonProperty("data")]
        public IDictionary<string, object> Data
        {
            get;
            set;
        }
    }
}