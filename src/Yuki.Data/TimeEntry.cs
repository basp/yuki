namespace Yuki.Data
{
    using System;
    using System.Collections.Generic;

    public class TimeEntry : IEntity
    {
        public TimeEntry()
        {
            this.LastUpdated = DateTime.UtcNow;
        }

        public int Id
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public int WorkspaceId
        {
            get;
            set;
        }

        public int? ProjectId
        {
            get;
            set;
        }

        public int? TaskId
        {
            get;
            set;
        }

        public DateTime Start
        {
            get;
            set;
        }

        public DateTime? Stop
        {
            get;
            set;
        }

        public int Duration
        {
            get;
            set;
        }

        public DateTime LastUpdated
        {
            get;
            private set;
        }

        public virtual ICollection<Tag> Tags
        {
            get;
            set;
        }
    }
}
