namespace Yuki.Commands
{
    public class SetupRequest
    {
        public string Server { get; set; }

        public string Folder { get; set; }

        public string RepositoryDatabase { get; set; }

        public string RepositorySchema { get; set; }

        public bool Restore { get; set; }
    }
}
