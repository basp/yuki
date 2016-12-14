namespace Yuki.Commands
{
    using System;
    using PowerArgs;

    public class RepositoryRequest : ISessionRequest, ISqlRepositoryConfig
    {
        [ArgRequired]
        [ArgPosition(1)]
        public string Server
        {
            get;
            set;
        }

        [ArgDefaultValue("yuki")]
        [ArgShortcut(CommonShortcuts.RepositoryDatabase)]
        public string RepositoryDatabase
        {
            get;
            set;
        }

        [ArgDefaultValue("dbo")]
        [ArgShortcut(CommonShortcuts.RepositorySchema)]
        public string RepositorySchema
        {
            get;
            set;
        }
    }
}
