namespace Yuki.Commands
{
    using PowerArgs;

    public class CreateRepositoryRequest : ISessionRequest
    {
        [ArgRequired]
        [ArgPosition(1)]
        [ArgShortcut(ArgShortcutPolicy.NoShortcut)]
        public string Server
        {
            get;
            set;
        }

        [ArgPosition(2)]
        [ArgDefaultValue("yuki")]
        [ArgShortcut(ArgShortcutPolicy.NoShortcut)]
        public string Database
        {
            get;
            set;
        }

        [ArgPosition(3)]
        [ArgDefaultValue("dbo")]
        [ArgShortcut(ArgShortcutPolicy.NoShortcut)]
        public string Schema
        {
            get;
            set;
        }
    }
}
