namespace Yuki.IdentityServer
{
    using IdentityServer3.Core;
    using IdentityServer3.Core.Models;
    using System.Collections.Generic;

    internal static class Scopes
    {
        public static List<Scope> Get()
        {
            return new List<Scope>
            {
                new Scope
                {
                    Name = "yuki",
                    Claims = new List<ScopeClaim>
                    {
                        new ScopeClaim(Constants.ClaimTypes.Email, alwaysInclude: true),
                    }
                },
            };
        }
    }
}