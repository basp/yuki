namespace Yuki.Commands
{
    using Newtonsoft.Json;

    public class CreateDatabaseResponse : DatabaseResponse
    {
        public CreateDatabaseResponse(string server, string database)
            : base(server, database)
        {
        }

        [JsonProperty(PropertyName = "created")]
        public bool Created { get; set; }
    }
}
