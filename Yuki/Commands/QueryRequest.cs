namespace Yuki.Commands
{
    using System;
    using System.Collections.Generic;
    using PowerArgs;
    using Optional;

    public class QueryRequest : ISessionRequest
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
        public Option<IDictionary<string, object>, Exception> Args
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
