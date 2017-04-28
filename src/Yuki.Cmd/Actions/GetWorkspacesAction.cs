namespace Yuki.Cmd.Actions
{
    using System;
    using System.Configuration;
    using System.Threading.Tasks;
    using Flurl;
    using Flurl.Http;
    using IdentityModel.Client;

    public class GetWorkspacesAction : IAction<GetWorkspacesArgs>
    {
        private static readonly string UserName =
            ConfigurationManager.AppSettings.Get("userName");

        private static readonly string Password =
            ConfigurationManager.AppSettings.Get("password");

        private readonly TokenClient tokenClient;

        public GetWorkspacesAction(TokenClient tokenClient)
        {
            this.tokenClient = tokenClient;
        }

        public async Task Execute(GetWorkspacesArgs args)
        {
            var response = GetClientToken();

            var result = await "http://localhost:52946/"
                .AppendPathSegments("api", "workspaces")
                .WithOAuthBearerToken(response.AccessToken)
                .GetStringAsync();

            Console.WriteLine(result);
        }

        private TokenResponse GetClientToken()
        {
            return this.tokenClient
                .RequestResourceOwnerPasswordAsync(UserName, Password, "yuki")
                .Result;
        }
    }
}
