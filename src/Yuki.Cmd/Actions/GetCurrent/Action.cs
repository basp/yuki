namespace Yuki.Cmd.Actions.GetCurrent
{
    using System;
    using System.Configuration;
    using System.Threading.Tasks;
    using Flurl;
    using Flurl.Http;
    using IdentityModel.Client;

    public class Action : IAction<Args>
    {
        private static readonly string UserName =
            ConfigurationManager.AppSettings.Get("userName");

        private static readonly string Password =
            ConfigurationManager.AppSettings.Get("password");

        private readonly TokenClient tokenClient;

        public Action(TokenClient tokenClient)
        {
            this.tokenClient = tokenClient;
        }

        public async Task Execute(Args args)
        {
            var response = GetClientToken();

            var result = await Config.Server
                .AppendPathSegments("api", "time_entries", "current")
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
