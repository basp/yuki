namespace Yuki.Api.TimeEntries.StopTimeEntry
{
    public class Request
    {
        public Request(int timeEntryId)
        {
            this.TimeEntryId = timeEntryId;
        }

        public int TimeEntryId { get; private set; }
    }
}