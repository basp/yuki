namespace Yuki.Api.TimeEntries.GetTimeEntryDetails
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