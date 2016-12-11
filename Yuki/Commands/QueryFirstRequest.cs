namespace Yuki.Commands
{
    using System;
    using System.Collections.Generic;
    using Optional;
    using PowerArgs;

    public class QueryFirstRequest : ISessionRequest
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
        public string Sql
        {
            get;
            set;
        }

        [ArgShortcut("f")]
        [ArgDefaultValue(false)]
        public bool Format
        {
            get;
            set;
        }
    }
}
