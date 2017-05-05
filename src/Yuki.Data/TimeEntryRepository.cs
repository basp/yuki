namespace Yuki.Data
{
    public class TimeEntryRepository : Repository<TimeEntry>
    {
        public TimeEntryRepository(DataContext context)
            : base(context)
        {
        }
    }
}
