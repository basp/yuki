namespace Yuki.Commands
{
    using PowerArgs;

    public class InsertVersionRequest : RepositoryRequest, IVersionRecord
    {
        [ArgRequired]
        [ArgShortcut(CommonShortcuts.RepositoryPath)]
        public string RepositoryPath { get; set; }

        [ArgRequired]
        [ArgShortcut(CommonShortcuts.VersionName)]
        public string VersionName { get; set; }

        [ArgIgnore]
        public string EnteredBy { get; set; }
    }
}
