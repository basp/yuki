namespace Yuki.Data
{
    using System.Linq;

    public class UserRepository : Repository<User>
    {
        public UserRepository(DataContext context)
            : base(context)
        {
        }

        public User Authenticate(string username, string password) =>
            this.context.Users
                .AsNoTracking()
                .Where(x => x.Username == username)
                .Where(x => x.Password == password)
                .FirstOrDefault();

        public User GetBySubject(string subject) =>
            this.context.Users
                .AsNoTracking()
                .FirstOrDefault(x => x.Email == subject);
    }
}
