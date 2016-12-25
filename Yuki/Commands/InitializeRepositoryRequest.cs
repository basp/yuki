namespace Yuki.Commands
{
    public class InitializeRepositoryRequest
    {
        public string Server { get; set; }

        public string RepositoryDatabase { get; set; }

        public string RepositorySchema { get; set; }
    }
}
