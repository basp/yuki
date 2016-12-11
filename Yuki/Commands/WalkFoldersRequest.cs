namespace Yuki.Commands
{
    using PowerArgs;

    public class WalkFoldersRequest
    {
        [ArgRequired]
        public string Folder { get; set; }
    }
}
