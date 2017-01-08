namespace Yuki
{
    using System.Diagnostics.Contracts;

    public class ScriptFolder
    {
        public ScriptFolder(
            string path,
            bool isOneTimeFolder,
            bool isEveryTimeFolder)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(path));

            this.Path = path;
            this.IsOneTimeFolder = isOneTimeFolder;
            this.IsEveryTimeFolder = isEveryTimeFolder;
        }

        public string Path { get; }

        public bool IsOneTimeFolder { get; }

        public bool IsEveryTimeFolder { get;  }
    }
}
