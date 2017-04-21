namespace Yuki.Model
{
    using System.Data.Entity;

    public class DataContext : DbContext
    {
        static DataContext()
        {
            var ignored = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        }

        public DataContext()
            : base("Yuki")
        {
        }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<Workspace> Workspaces { get; set; }

        public virtual DbSet<Project> Projects { get; set; }

        public virtual DbSet<Entry> Entries { get; set; }

        public virtual DbSet<Timer> Timers { get; set; }
    }
}
