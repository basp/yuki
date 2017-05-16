using System;

namespace Yuki.Data
{
    public class Project : IEntity
    {
        public Project()
        {
            this.LastUpdated = DateTime.UtcNow;
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

        public int WorkspaceId
        {
            get;
            set;
        }

        public int? ClientId
        {
            get;
            set;
        }

        public bool IsActive
        {
            get;
            set;
        }

        public bool IsPrivate
        {
            get;
            set;
        }

        public DateTime LastUpdated
        {
            get;
            private set;
        }
    }
}
