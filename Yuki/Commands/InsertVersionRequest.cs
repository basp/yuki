namespace Yuki.Commands
{
    public class InsertVersionRequest
    {
        public string Server { get; set; }

        public string RepositoryDatabase { get; set; }

        public string RepositorySchema { get; set; }

        public string RepositoryPath { get; set; }

        public string RepositoryVersion { get; set; }
    }
}
