namespace Yuki.Commands
{
    public class MigrateResponse
    {
        public string Server { get; set; }

        public string RepositoryPath { get; set; }

        public string OldVersion { get; set; }

        public string NewVersion { get; set; }
    }
}
