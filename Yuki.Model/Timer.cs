namespace Yuki.Model
{
    using System;

    public class Timer
    {
        private Timer()
        {
        }

        public Timer(int workspaceId, int userId)
        {
            this.WorkspaceId = workspaceId;
            this.UserId = userId;
            this.Started = DateTime.UtcNow;
        }

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

        public int UserId
        {
            get;
            set;
        }

        public DateTime Started
        {
            get;
            set;
        }
    }
}
