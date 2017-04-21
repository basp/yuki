namespace Yuki.Model
{
    using System;

    public class Entry
    {
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

        public virtual User User
        {
            get;
            private set;
        }
    }
}
