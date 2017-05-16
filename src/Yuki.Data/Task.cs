namespace Yuki.Data
{
    using System;

    public class Task
    {
        public Task()
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

        public int ProjectId
        {
            get;
            set;
        }

        public bool IsActive
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
