namespace Yuki.Commands
{
    using Optional;

    public class SetupDatabaseResponse
    {
        public string Server { get; set; }

        public string Database { get; set; }

        public string Folder { get; set; }

        public string Backup { get; set; }
    }
}
