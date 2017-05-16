namespace Yuki.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<DataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DataContext context)
        {
            context.Users.AddOrUpdate(x => x.Email, new[]
            {
                new User
                {
                    Email = "jd@sandbox.com",
                    FullName = "John Doe",
                }
            });
        }
    } 
}
