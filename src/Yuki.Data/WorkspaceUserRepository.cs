namespace Yuki.Data
{
    using System.Collections.Generic;
    using System.Linq;

    public class WorkspaceUserRepository
    {
        private readonly DataContext dataContext;

        public WorkspaceUserRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public IEnumerable<WorkspaceUser> GetWorkspacesByUserId(int userId) =>
            this.dataContext.WorkspaceUsers
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .ToList();
    }
}
