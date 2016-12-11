namespace Yuki.Commands
{
    public class DropDatabaseResponse : DatabaseResponse
    {
        public DropDatabaseResponse(string server, string database)
            : base(server, database)
        {
        }
    }
}
