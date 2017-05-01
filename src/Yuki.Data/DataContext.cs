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

        public virtual DbSet<Client> Clients { get; set; }
    }
}
