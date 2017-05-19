namespace Yuki.Api.TimeEntries.GetTimeEntries
{
    using System;

    public class Request
    {
        public int UserId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}