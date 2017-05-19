namespace Yuki.Api
{
    using System.Collections.Generic;
    using IdentityServer3.Core.Models;

    public static class ApiClients
    {
        public static List<Client> Get() =>
            new List<Client>
            {
                new Client
                {
                    ClientName = "Yuki Command",
                    ClientId = "yukicmd",
                    Enabled = true,
                    AccessTokenType = AccessTokenType.Reference,

                    Flow = Flows.ResourceOwner,

                    ClientSecrets = new List<Secret>
                    {
                       // TODO
                       new Secret("frotz".Sha256()),
                    },

                    AllowedScopes = new List<string>
                    {
                        "api",
                    },
                },
            };
    }
}