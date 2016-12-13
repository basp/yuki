namespace Yuki.Commands
{
    using System;
    using System.IO;
    using PowerArgs;

    public class SetupDatabaseRequest
    {
        [ArgRequired]
        [ArgShortcut(ArgShortcutPolicy.NoShortcut)]
        [ArgPosition(1)]
        public string Server { get; set; }

        [ArgRequired]
        [ArgShortcut(ArgShortcutPolicy.NoShortcut)]
        [ArgPosition(2)]
        public string Folder { get; set; }

        [ArgShortcut(ArgShortcutPolicy.NoShortcut)]
        [ArgDefaultValue(false)]
        public bool Restore { get; set; }

        [ArgDefaultValue(300)]
        [ArgShortcut(CommonShortcuts.RestoreTimeout)]
        public string RestoreTimeout { get; set; }

        [ArgIgnore]
        public string Database
        {
            get
            {
                return Path.GetFileName(this.Folder);
            }
        }
    }
}
