namespace Yuki.Model
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

        public virtual DbSet<Entry> Entries
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

        public virtual DbSet<Timer> Timers
        {
            get;
            set;
        }

        public virtual DbSet<User> Users
        {
            get;
            set;
        }

        public virtual DbSet<Workspace> Workspaces
        {
            get;
            set;
        }
    }
}
