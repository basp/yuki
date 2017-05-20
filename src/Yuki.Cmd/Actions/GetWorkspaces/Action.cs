namespace Yuki.Cmd.Actions.GetWorkspaces
{
    using System;
    using System.Threading.Tasks;
    using Flurl;
    using Flurl.Http;
    using IdentityModel.Client;

    public class Action : IAction<Args>
    {
        private readonly TokenClient tokenClient;

        public Action(TokenClient tokenClient)
        {
            this.tokenClient = tokenClient;
        }

        public async Task Execute(Args args)
        {
            var tokenResponse = this.GetClientToken(tokenClient);

            var result = await Config.ApiEndPoint
                .AppendPathSegments("api", "workspaces")
                .WithOAuthBearerToken(tokenResponse.AccessToken)
                .GetStringAsync();

            Console.WriteLine(result);
        }
    }
}
