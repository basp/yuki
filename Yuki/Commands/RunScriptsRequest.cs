namespace Yuki.Commands
{
    public class RunScriptsRequest
    {
        public string Folder { get; set; }

        public int VersionId { get; set; }

        public string RepositoryVersion { get; set; }

        public bool IsOneTimeFolder { get; set; }

        public bool IsEveryTimeFolder { get; set; }
    }
}
