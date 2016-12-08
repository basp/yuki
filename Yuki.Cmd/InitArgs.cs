namespace Yuki.Cmd
{
    using PowerArgs;

    public class InitArgs
    {
        [ArgDescription("Directory to initilialize (will be created if necessary)")]
        [ArgDefaultValue(".")]
        [ArgShortcut("f")]
        [ArgPosition(1)]
        public string Folder { get; set; }

        [ArgDescription("Force in a folder that is not empty")]
        [ArgShortcut(ArgShortcutPolicy.NoShortcut)]
        [ArgDefaultValue(false)]
        public bool Force { get; set; }

        [ArgDescription("Overwrite any existing files")]
        [ArgShortcut(ArgShortcutPolicy.NoShortcut)]
        [ArgDefaultValue(false)]
        public bool Overwrite { get; set; }
    }
}