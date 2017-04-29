namespace Yuki.Data
{
    using System;
    using System.Collections.Generic;

    public class Workspace
    {
        public Workspace()
        {
            this.Users = new List<User>();
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

        public virtual ICollection<User> Users
        {
            get;
            private set;
        }
    }
}
