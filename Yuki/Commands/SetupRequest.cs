namespace Yuki.Commands
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
        public string DatabasesFolder { get; set; }

        [ArgDefaultValue("yuki")]
        [ArgShortcut(CommonShortcuts.RepositoryDatabase)]
        public string RepositoryDatabase { get; set; }

        [ArgDefaultValue("dbo")]
        [ArgShortcut(CommonShortcuts.RepositorySchema)]
        public string RepositorySchema { get; set; }

        [ArgShortcut(ArgShortcutPolicy.NoShortcut)]
        [ArgDefaultValue(false)]
        public bool Restore { get; set; }

        [ArgDefaultValue(300)]
        [ArgShortcut(CommonShortcuts.RestoreTimeout)]
        public int RestoreTimeout { get; set; }
    }
}
