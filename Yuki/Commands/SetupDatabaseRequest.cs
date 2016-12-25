namespace Yuki.Commands
{
    public class SetupDatabaseRequest
    {
        public string Server { get; set; }

        public string Folder { get; set; }

        public bool Restore { get; set; }
    }
}
