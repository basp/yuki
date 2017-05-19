namespace Yuki.Api.TimeEntries.GetTimeEntries
{
    using System;

    public class Request
    {
        public Request(DateTime startDate, DateTime endDate)
        {
            this.StartDate = startDate;
            this.EndDate = endDate;
        }

        public int UserId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public Request WithUserId(int userId)
        {
            this.UserId = userId;
            return this;
        }
    }
}