namespace Yuki.Commands
{
    public class RestoreDatabaseResponse : DatabaseResponse
    {
        public RestoreDatabaseResponse(string server, string database, string backup)
            : base(server, database)
        {
            this.Backup = backup;
        }

        public string Backup
        {
            get;
            set;
        }
    }
}
