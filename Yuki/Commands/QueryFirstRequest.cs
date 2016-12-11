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

        [ArgPosition(3)]
        [ArgShortcut(ArgShortcutPolicy.NoShortcut)]
        public Option<IDictionary<string, string>, Exception> Params
        {
            get;
            set;
        }
    }
}
