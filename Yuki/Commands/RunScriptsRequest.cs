namespace Yuki.Commands
{
    public class RunScriptsRequest
    {
        public string RepositoryDatabase { get; set; }

        public string RepositorySchema { get; set; }

        public string ProjectFolder { get; set; }

        public string ScriptFolder { get; set; }

        public int VersionId { get; set; }

        public string RepositoryVersion { get; set; }

        public bool IsOneTimeFolder { get; set; }

        public bool IsEveryTimeFolder { get; set; }
    }
}
