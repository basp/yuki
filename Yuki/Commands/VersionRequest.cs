namespace Yuki.Commands
{
    using PowerArgs;

    public class VersionRequest : RepositoryRequest
    {
        [ArgRequired]
        [ArgShortcut(CommonShortcuts.RepositoryPath)]
        public string RepositoryPath { get; set; }

        [ArgShortcut(CommonShortcuts.VersionFile)]
        [ArgDefaultValue("version.txt")]
        public string VersionFile { get; set; }
    }
}
