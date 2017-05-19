namespace Yuki.Data
{
    using System.Linq;

    public class UserRepository : Repository<User>
    {
        public UserRepository(DataContext context)
            : base(context)
        {
        }

        public User GetBySubject(string subject)
        {
            return this.context.Users
                .AsNoTracking()
                .FirstOrDefault(x => x.Email == subject);
        }
    }
}
