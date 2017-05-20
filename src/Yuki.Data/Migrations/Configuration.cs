namespace Yuki.Data.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<DataContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DataContext context)
        {
            context.Users.AddOrUpdate(x => x.Email, new[]
            {
                new User
                {
                    Username = "test",
                    Password = "test",
                    Email = "test@test.com",
                    FullName = "Test User",
                },
            });

            context.Workspaces.AddOrUpdate(x => x.Id, new[]
            {
                new Workspace
                {
                    Id = 1,
                    Name = "Sandbox",
                },
            });
        }
    }
}
