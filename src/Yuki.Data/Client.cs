namespace Yuki.Data
{
    using System;

    public class Client : IEntity
    {
        public Client()
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

        public string Notes
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
