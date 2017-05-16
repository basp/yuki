using System.Linq;

namespace Yuki.Data
{
    public class TimeEntryRepository : Repository<TimeEntry>
    {
        public TimeEntryRepository(DataContext context)
            : base(context)
        {
        }

        public TimeEntry GetCurrent(int userId)
        {
            return this.context.TimeEntries
                .AsNoTracking()
                .FirstOrDefault(x => x.UserId == userId);
        }
    }
}
