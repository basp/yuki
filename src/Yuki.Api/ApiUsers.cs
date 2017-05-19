namespace Yuki.Api
{
    using System.Collections.Generic;
    using IdentityServer3.Core.Services.InMemory;

    public static class ApiUsers
    {
        public static List<InMemoryUser> Get() =>
            new List<InMemoryUser>
            {
                new InMemoryUser
                {
                    Username = "test",
                    Password = "test",
                    Subject = "test@foo.com",
                },
            };
    }
}