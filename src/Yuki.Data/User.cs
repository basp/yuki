namespace Yuki.Data
{
    using System;

    public class User : IEntity
    {
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
