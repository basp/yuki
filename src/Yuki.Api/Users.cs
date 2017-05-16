namespace Yuki.Api
{
    using IdentityModel;
    using IdentityServer4.Test;
    using System.Collections.Generic;
    using System.Security.Claims;

    internal static class Users
    {
        public static List<TestUser> Get() =>
            new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "5BE86359-073C-434B-AD2D-A3932222DABE",
                    Username = "test",
                    Password = "password",
                    Claims = new List<Claim>
                    {
                        new Claim(JwtClaimTypes.Email, "test@test.com"),
                        new Claim(JwtClaimTypes.Role, "admin"),
                    },
                }
            };
    }
}
