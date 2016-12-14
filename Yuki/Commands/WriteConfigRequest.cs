namespace Yuki.Commands
{
    using PowerArgs;

    public class WriteConfigRequest
    {
        [ArgRequired]
        [ArgPosition(1)]
        [ArgShortcut(ArgShortcutPolicy.NoShortcut)]
        public string File { get; set; }
    }
}
