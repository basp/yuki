namespace Yuki.Commands
{
    public class MigrateRequest
    {
        public string Server { get; set; }

        public string RepositoryDatabase { get; set; }

        public string RepositorySchema { get; set; }

        public string RepositoryPath { get; set; }

        public string VersionFile { get; set; }
    }
}
