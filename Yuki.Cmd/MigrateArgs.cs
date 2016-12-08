namespace Yuki.Cmd
{
    using System.Collections.Generic;
    using System.Linq;
    using PowerArgs;

    public class MigrateArgs
    {
        [ArgDescription("The database server to update")]
        [ArgShortcut("s")]
        [ArgRequired]
        [ArgPosition(1)]
        public string Server { get; set; }

        [ArgDescription("Source repository path")]
        [ArgShortcut("r")]
        [ArgRequired]
        [ArgPosition(2)]
        public string Repository { get; set; }

        [ArgDescription("Version file")]
        [ArgShortcut("vf")]
        [ArgDefaultValue("version.txt")]
        public string VersionFile { get; set; }

        [ArgDescription("Path to a Yuki config file")]
        [ArgDefaultValue(Context.DefaultConfigFile)]
        [ArgShortcut("cfg")]
        public string Config { get; set; }

        [ArgDescription("The environment that is migrated")]
        [ArgShortcut("env")]
        [ArgDefaultValue("local")]
        public string Environment { get; set; }

        [ArgDescription("A comma-separated string of tokens")]
        [ArgShortcut("ts")]
        [ArgDefaultValue("")]
        public IDictionary<string, string> Tokens { get; set; }

        [ArgDescription("Global command timeout")]
        [ArgShortcut("ct")]
        [ArgDefaultValue(30)]
        public string CommandTimeout { get; set; }

        [ArgDescription("Restore timeout")]
        [ArgShortcut("rt")]
        [ArgDefaultValue(5 * 30)]
        public int RestoreTimeout { get; set; }

        [ArgShortcut("tx")]
        [ArgDefaultValue(true)]
        [ArgDescription("Run the migration process in a transaction")]
        public bool RunInTransaction { get; set; }

        [ArgDescription("Warn instead of panic on one-time script changes")]
        [ArgShortcut(ArgShortcutPolicy.NoShortcut)]
        [ArgDefaultValue(false)]
        public bool WarnOnOneTimeScriptChanges { get; set; }

        [ArgDescription("Skip the run before update step")]
        [ArgShortcut(ArgShortcutPolicy.NoShortcut)]
        [ArgDefaultValue(false)]
        public bool SkipRunBeforeUp { get; set; }

        [ArgReviver]
        public static IDictionary<string, string> TokensArgReviver(
            string name,
            string value)
        {
            // If this fails we don't get a fancy exception by default
            // Just: "Exception has been thrown by the target of an invocation"

            if (string.IsNullOrEmpty(value))
            {
                return new Dictionary<string, string>();
            }

            return value.Split(',')
                .Select(x => x.Trim())
                .Select(x => x.Split('=').Select(y => y.Trim()).ToArray())
                .ToDictionary(x => x[0], x => x[1]);
        }
    }
}
