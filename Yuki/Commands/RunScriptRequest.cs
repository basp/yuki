namespace Yuki.Commands
{
    public class RunScriptRequest
    {
        public string Server { get; set; }

        public string RepositoryDatabase { get; set; }

        public string RepositorySchema { get; set; }

        public bool IsOneTimeFolder { get; set; }

        public bool IsEveryTimeFolder { get; set; }
    }
}
