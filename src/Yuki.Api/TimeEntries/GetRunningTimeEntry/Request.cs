namespace Yuki.Api.TimeEntries.GetRunningTimeEntry
{
    public class Request
    {
        public Request(int userId)
        {
            this.UserId = userId;
        }

        public int UserId
        {
            get;
            private set;
        }
    }
}