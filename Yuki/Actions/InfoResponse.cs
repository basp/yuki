namespace Yuki.Actions
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class InfoResponse
    {
        [JsonProperty(PropertyName = "messages")]
        public IEnumerable<string> Messages { get; set; }

        [JsonProperty(PropertyName = "config")]
        public Config Config { get; set; }
    }
}
