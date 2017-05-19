namespace Yuki.Api.TimeEntries.UpdateTimeEntry
{
    using System.Collections.Generic;

    public class Request
    {
        public int TimeEntryId { get; set; }

        public IDictionary<string,object> TimeEntry { get; set; }
    }
}