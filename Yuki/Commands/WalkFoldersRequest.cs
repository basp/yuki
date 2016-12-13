namespace Yuki.Commands
{
    using PowerArgs;

    public class WalkFoldersRequest
    {
        [ArgRequired]
        [ArgPosition(1)]
        [ArgShortcut(ArgShortcutPolicy.NoShortcut)]
        public string Folder { get; set; }
    }
}
