namespace Yuki.IdentityServer
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using IdentityServer3.Core;
    using IdentityServer3.Core.Services.InMemory;

    internal static class Users
    {
        public static List<InMemoryUser> Get()
        {
            return new List<InMemoryUser>
            {
                new InMemoryUser
                {
                    Username = "admin",
                    Password = "secret",
                    Subject = "1",

                    Claims = new []
                    {
                        new Claim(Constants.ClaimTypes.Email, "foo@bar.com"),
                    }
                }
            };
        }
    }
}