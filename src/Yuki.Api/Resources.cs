namespace Yuki.Api
{
    using System.Collections.Generic;
    using IdentityServer4.Models;

    internal static class Resources
    {
        public static IEnumerable<IdentityResource> GetIdentityResources() =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResource
                {
                    Name = "role",
                    UserClaims = new List<string>
                    {
                        "role",
                    },
                },
            };

        public static IEnumerable<ApiResource> GetApiResources() =>
            new List<ApiResource>
            {
                new ApiResource
                {
                    Name = "api",
                    DisplayName = "Yuki API",
                    Description = "Yuki API access",
                    UserClaims = new List<string>
                    {
                        "role",
                    },
                    ApiSecrets = new List<Secret>
                    {
                        new Secret("scopeSecret".Sha256()),
                    },
                    Scopes = new List<Scope>
                    {
                        new Scope("api.read"),
                        new Scope("api.write"),
                    },
                },
            };
    }
}
