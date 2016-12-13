namespace Yuki.Commands
{
    using PowerArgs;

    public class GetVersionRequest : RepositoryRequest
    {
        [ArgRequired]
        [ArgShortcut("rp")]
        public string RepositoryPath { get; set; }
    }
}
