namespace Yuki.Data
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    public class User : IEntity
    {
        public User()
        {
            this.LastUpdated = DateTime.UtcNow;
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

        public string FullName
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
