namespace Yuki.Model
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

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

        public string DisplayName
        {
            get;
            set;
        }

        [Index(IsUnique = true)]
        [MaxLength(256)]
        public string Email
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
