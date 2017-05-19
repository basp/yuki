namespace Yuki.Data
{
    using System.Collections.Generic;
    using System.Linq;

    public class WorkspaceRepository : Repository<Workspace>
    {
        public WorkspaceRepository(DataContext dataContext)
            : base(dataContext)
        {
        }

        public IEnumerable<Workspace> GetWorkspaces(params int[] ids) =>
            this.context.Workspaces
                .AsNoTracking()
                .Where(x => ids.Contains(x.Id))
                .ToList();
    }
}
