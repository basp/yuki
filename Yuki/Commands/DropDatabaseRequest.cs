namespace Yuki.Commands
{
    using PowerArgs;

    public class DropDatabaseRequest : ISessionRequest
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
    }
}
