namespace Yuki.Commands
{
    using PowerArgs;

    public class HashFileRequest
    {
        [ArgRequired]
        [ArgPosition(1)]
        [ArgShortcut(ArgShortcutPolicy.NoShortcut)]
        [ArgExistingFile]
        public string File
        {
            get;
            set;
        }
    }
}
