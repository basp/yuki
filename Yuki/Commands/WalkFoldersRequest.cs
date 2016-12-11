namespace Yuki.Commands
{
    using PowerArgs;

    public class WalkFoldersRequest
    {
        [ArgRequired]
        public string Directory { get; set; }
    }
}
