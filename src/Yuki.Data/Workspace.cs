namespace Yuki.Data
{
    using System;
    using System.Collections.Generic;

    public class Workspace : IEntity
    {
        public Workspace()
        {
            this.Users = new List<WorkspaceUser>();
        }

        public int Id
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public DateTime LastUpdated
        {
            get;
            private set;
        }

        public virtual ICollection<WorkspaceUser> Users
        {
            get;
            private set;
        }
    }
}
