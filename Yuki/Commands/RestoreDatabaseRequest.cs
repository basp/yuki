namespace Yuki.Commands
{
    using System.Diagnostics.Contracts;
    using PowerArgs;

    public class RestoreDatabaseRequest : DatabaseRequest
    {
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
                Backup = backup,
            };
        }
    }
}
