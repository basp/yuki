namespace Yuki.Model
{
    using System;
    using System.Collections.Generic;

    public class Entry
    {
        public Entry()
        {
            this.Tags = new List<Tag>();
        }

        public int Id
        {
            get;
            set;
        }

        public int UserId
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

        public string Description
        {
            get;
            set;
        }

        public TimeSpan Duration
        {
            get;
            set;
        }

        public virtual ICollection<Tag> Tags
        {
            get;
            private set;
        }

        public virtual User User
        {
            get;
            private set;
        }
    }
}
