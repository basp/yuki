namespace Yuki.Model
{
    using System.Collections.Generic;

    public class User
    {
        public User()
        {
            this.Entries = new List<Entry>();
        }

        public int Id
        {
            get;
            set;
        }

        public string Email
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
