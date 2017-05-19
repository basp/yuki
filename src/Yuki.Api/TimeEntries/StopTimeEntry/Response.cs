namespace Yuki.Api.TimeEntries.StopTimeEntry
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class Response
    {
        public Response(IDictionary<string, object> data)
        {
            this.Data = data;
        }

        [JsonProperty("data")]
        public IDictionary<string, object> Data { get; private set; }
    }
}