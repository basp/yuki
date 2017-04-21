namespace Yuki.Model
{
    using System.Collections.Generic;

    public class Project
    {
        public int Id
        {
            get;
            set;
        }

        public int WorkspaceId
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public virtual ICollection<Entry> Entries
        {
            get;
            private set;
        }
    }
}
