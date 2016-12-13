namespace Yuki
{
    using PowerArgs;

    public class SetupRequest
    {
        [ArgRequired]
        [ArgPosition(1)]
        [ArgShortcut(ArgShortcutPolicy.NoShortcut)]
        public string Server { get; set; }

        [ArgRequired]
        [ArgPosition(2)]
        [ArgShortcut(ArgShortcutPolicy.NoShortcut)]
        public string Folder { get; set; }

        [ArgShortcut(ArgShortcutPolicy.NoShortcut)]
        [ArgDefaultValue(false)]
        public bool Restore { get; set; }

        [ArgDefaultValue(300)]
        [ArgShortcut(CommonShortcuts.RestoreTimeout)]
        public int RestoreTimeout { get; set; }
    }
}
