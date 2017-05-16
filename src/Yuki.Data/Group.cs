namespace Yuki.Data
{
    using System;

    public class Group : IEntity
    {
        public Group()
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

        public DateTime LastUpdated
        {
            get;
            private set;
        }
    }
}
