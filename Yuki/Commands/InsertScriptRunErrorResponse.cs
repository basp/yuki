namespace Yuki.Commands
{
    using Newtonsoft.Json;

    public class InsertScriptRunErrorResponse : IRepositoryResponse
    {
        [JsonProperty(PropertyName = "server")]
        public string Server { get; set; }

        [JsonProperty(PropertyName = "database")]
        public string Database { get; set; }

        [JsonProperty(PropertyName = "schema")]
        public string Schema { get; set; }
    }
}
