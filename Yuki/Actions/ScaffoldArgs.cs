namespace Yuki.Actions
{
    using PowerArgs;

    public class ScaffoldArgs
    {
        [ArgDescription("Path to a Yuki config file")]
        [ArgDefaultValue(Default.ConfigFile)]
        [ArgShortcut("cfg")]
        public string Config { get; set; }

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