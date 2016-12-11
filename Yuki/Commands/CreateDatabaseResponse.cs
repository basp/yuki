namespace Yuki.Commands
{
    public class CreateDatabaseResponse : DatabaseResponse
    {
        public CreateDatabaseResponse(string server, string database)
            : base(server, database)
        {
        }
        public bool Created { get; set; }
    }
}
