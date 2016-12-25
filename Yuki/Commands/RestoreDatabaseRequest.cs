namespace Yuki.Commands
{
    public class RestoreDatabaseRequest
    {
        public string Server { get; set; }

        public string Database { get; set; }

        public string Backup { get; set; }
    }
}