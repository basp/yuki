namespace Yuki.Model
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    public class Repository
    {
        private readonly DataContext context;

        public Repository(DataContext context)
        {
            this.context = context;
        }

        public void DeleteTimer(Timer timer)
        {
            this.context.Timers.Attach(timer);
            this.context.Entry(timer).State = EntityState.Deleted;
            this.context.SaveChanges();
        }

        public void DeleteUser(User user)
        {
            this.context.Users.Attach(user);
            this.context.Entry(user).State = EntityState.Deleted;
            this.context.SaveChanges();
        }

        public void DeleteWorkspace(Workspace workspace)
        {
            this.context.Workspaces.Attach(workspace);
            this.context.Entry(workspace).State = EntityState.Deleted;
            this.context.SaveChanges();
        }

        public IEnumerable<User> GetAllUsers()
        {
            return this.context.Users
                .AsNoTracking()
                .ToList();
        }

        public Entry GetEntry(int entryId)
        {
            return this.context.Entries
                .AsNoTracking()
                .Include(x => x.User)
                .Where(x => x.Id == entryId)
                .FirstOrDefault();
        }

        public User GetUser(int userId)
        {
            return this.context.Users
                .AsNoTracking()
                .Where(x => x.Id == userId)
                .FirstOrDefault();
        }

        public Workspace GetWorkspace(int workspaceId)
        {
            return this.context.Workspaces
                .AsNoTracking()
                .Include(x => x.Projects)
                .Where(x => x.Id == workspaceId)
                .FirstOrDefault();
        }

        public Timer GetTimer(int timerId)
        {
            return this.context.Timers
                .AsNoTracking()
                .Where(x => x.Id == timerId)
                .FirstOrDefault();
        }

        public IEnumerable<Timer> GetWorkspaceTimers(int workspaceId)
        {
            return this.context.Timers
                .AsNoTracking()
                .Where(x => x.WorkspaceId == workspaceId)
                .ToList();
        }

        public Timer GetUserTimer(int userId)
        {
            return this.context.Timers
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .FirstOrDefault();
        }

        public IEnumerable<Workspace> GetAllWorkspaces()
        {
            return this.context.Workspaces
                .AsNoTracking()
                .ToList();
        }

        public void InsertProject(Project project)
        {
            this.context.Projects.Add(project);
            this.context.SaveChanges();
        }

        public void InsertTimer(Timer timer)
        {
            this.context.Timers.Add(timer);
            this.context.SaveChanges();
        }

        public void InsertEntry(Entry entry)
        {
            this.context.Entries.Add(entry);
            this.context.SaveChanges();
        }

        public void InsertUser(User user)
        {
            this.context.Users.Add(user);
            this.context.SaveChanges();
        }

        public void InsertWorkspace(Workspace workspace)
        {
            this.context.Workspaces.Add(workspace);
            this.context.SaveChanges();
        }

        public void UpdateProject(Project project)
        {
            this.context.Projects.Attach(project);
            this.context.Entry(project).State = EntityState.Modified;
            this.context.SaveChanges();
        }

        public void UpdateTimer(Timer timer)
        {
            this.context.Timers.Attach(timer);
            this.context.SaveChanges();
        }

        public void UpdateUser(User user)
        {
            this.context.Users.Attach(user);
            this.context.Entry(user).State = EntityState.Modified;
            this.context.SaveChanges();
        }

        public void UpdateWorkspace(Workspace workspace)
        {
            this.context.Workspaces.Attach(workspace);
            this.context.Entry(workspace).State = EntityState.Modified;
            this.context.SaveChanges();
        }
    }
}
