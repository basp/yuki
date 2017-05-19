namespace Yuki.Api
{
    using System.Collections.Generic;
    using IdentityServer3.Core.Models;

    public static class ApiScopes
    {
        public static List<Scope> Get() =>
            new List<Scope>
            {
                new Scope { Name = "api" },
            };
    }
}