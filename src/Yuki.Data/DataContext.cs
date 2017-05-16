namespace Yuki.Data
{
    using System.Data.Entity;
    using System.Data.Entity.SqlServer;

    public class DataContext : DbContext
    {
        static DataContext()
        {
            var _ = SqlProviderServices.Instance;
        }

        public DataContext()
            : base("Yuki")
        {
        }

        public virtual DbSet<Client> Clients
        {
            get;
            set;
        }

        public virtual DbSet<Group> Groups
        {
            get;
            set;
        }

        public virtual DbSet<Project> Projects
        {
            get;
            set;
        }

        public virtual DbSet<Tag> Tags
        {
            get;
            set;
        }

        public virtual DbSet<Task> Tasks
        {
            get;
            set;
        }

        public virtual DbSet<TimeEntry> TimeEntries
        {
            get;
            set;
        }

        public virtual DbSet<Workspace> Workspaces
        {
            get;
            set;
        }

        public virtual DbSet<User> Users
        {
            get;
            set;
        }

        public virtual DbSet<WorkspaceUser> WorkspaceUsers
        {
            get;
            set;
        }
    }
}