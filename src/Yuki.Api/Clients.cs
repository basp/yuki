namespace Yuki.Api
{
    using IdentityServer4.Models;
    using System.Collections.Generic;

    internal static class Clients
    {
        public static IEnumerable<Client> Get() =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "console",
                    ClientName = "Console Client",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("secret".Sha256()),
                    },
                    AllowedScopes = new List<string>
                    {
                        "api.read",
                    },
                }
            };
    }
}
