namespace Yuki.Commands
{
    using System.Diagnostics.Contracts;
    using PowerArgs;

    public class RestoreDatabaseRequest : ISessionRequest
    {
        [ArgRequired]
        [ArgPosition(1)]
        [ArgShortcut(ArgShortcutPolicy.NoShortcut)]
        public string Server
        {
            get;
            set;
        }

        [ArgRequired]
        [ArgPosition(2)]
        [ArgShortcut(ArgShortcutPolicy.NoShortcut)]
        public string Database
        {
            get;
            set;
        }

        [ArgRequired]
        [ArgPosition(3)]
        [ArgExistingFile]
        [ArgShortcut(ArgShortcutPolicy.NoShortcut)]
        public string Backup
        {
            get;
            set;
        }

        public static RestoreDatabaseRequest Create(
            string database,
            string backup)
        {
            Contract.Requires(!string.IsNullOrEmpty(database));
            Contract.Requires(!string.IsNullOrEmpty(backup));

            return new RestoreDatabaseRequest()
            {
                Database = database,
                Backup = backup
            };
        }
    }
}
