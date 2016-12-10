﻿namespace Yuki.Commands
{
    using PowerArgs;

    public class CreateRepositoryRequest : DatabaseRequest, ISessionRequest
    {
        [ArgPosition(2)]
        [ArgDefaultValue("yuki")]
        [ArgShortcut(ArgShortcutPolicy.NoShortcut)]
        public override string Database
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
