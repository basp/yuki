namespace Yuki.Commands
{
    public class SetupDatabaseResponse
    {
        public string Server { get; set; }

        public string Folder { get; set; }

        public string Database { get; set; }

        public bool Created { get; set; }

        public bool Restored { get; set; }

        public string Backup { get; set; }
    }
}
