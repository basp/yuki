namespace Yuki.Commands
{
    public class RestoreDatabaseResponse : DatabaseResponse
    {
        public RestoreDatabaseResponse(string server, string database)
            : base(server, database)
        {
        }
    }
}
