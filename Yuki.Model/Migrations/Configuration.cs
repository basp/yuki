
namespace Yuki.Model.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<DataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DataContext context)
        {
            var workspace = new Workspace
            {
                Id = 1,
                Name = "Default",
            };

            var user = new User
            {
                Id = 1,
                Email = "foo@bar.com",
                Name = "Foo Bar",
            };

            context.Workspaces.AddOrUpdate(workspace);
            context.Users.AddOrUpdate(user);
        }
    }
}
