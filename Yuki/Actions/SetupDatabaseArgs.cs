namespace Yuki.Actions
{
    using PowerArgs;

    public class SetupDatabaseArgs
    {
        [ArgRequired]
        [ArgShortcut(ArgShortcutPolicy.NoShortcut)]
        [ArgPosition(1)]
        public ISession Server { get; set; }

        [ArgDescription("Folder containing the database specific scripts and/or backups to restore")]
        [ArgDefaultValue(Default.DatabaseFolder)]
        [ArgShortcut(ArgShortcutPolicy.NoShortcut)]
        [ArgPosition(2)]
        public string Folder { get; set; }

        [ArgReviver]
        public static ISession SessionReviver(string name, string value)
        {
            var cs = $"Server={value};Integrated Security=SSPI";
            return SqlSession.Create(cs);
        }
    }
}
