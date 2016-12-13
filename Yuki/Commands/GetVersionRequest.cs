namespace Yuki.Commands
{
    using PowerArgs;

    public class GetVersionRequest : RepositoryRequest
    {
        [ArgRequired]
        [ArgShortcut(CommonShortcuts.RepositoryPath)]
        public string RepositoryPath { get; set; }
    }
}
