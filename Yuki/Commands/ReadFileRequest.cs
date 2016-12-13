namespace Yuki.Commands
{
    using PowerArgs;

    public class ReadFileRequest
    {
        [ArgRequired]
        [ArgPosition(1)]
        [ArgShortcut(ArgShortcutPolicy.NoShortcut)]
        public string Path { get; set; }
    }
}
