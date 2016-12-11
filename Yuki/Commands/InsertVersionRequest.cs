using PowerArgs;

namespace Yuki.Commands
{
    public class InsertVersionRequest : RepositoryRequest, IVersionRecord
    {
        [ArgRequired]
        [ArgShortcut("rp")]
        public string RepositoryPath { get; set; }

        [ArgRequired]
        [ArgShortcut("vn")]
        public string VersionName { get; set; }

        [ArgIgnore]
        public string EnteredBy { get; set; }
    }
}
