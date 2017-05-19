namespace Yuki.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class TimeEntryRepository : Repository<TimeEntry>
    {
        public TimeEntryRepository(DataContext context)
            : base(context)
        {
        }

        public IEnumerable<TimeEntry> GetEntries(
            DateTime startDate,
            DateTime endDate,
            int limit = 1000) =>
            this.context.TimeEntries
                .AsNoTracking()
                .Where(x => x.Start >= startDate && x.Stop < endDate)
                .Take(limit)
                .ToList();


        public TimeEntry GetCurrent(int userId) =>
            this.context.TimeEntries
                .AsNoTracking()
                .FirstOrDefault(x => x.UserId == userId);
    }
}
