namespace Yuki.Cmd.Actions.StartTimeEntry
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Flurl;
    using Flurl.Http;
    using IdentityModel.Client;
    using Newtonsoft.Json;

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

            var data = new Dictionary<string, object>
            {
                ["wid"] = args.Wid,
                ["description"] = args.Description,
            };

            var result = await Config.ApiEndPoint
                .AppendPathSegments("api", "time_entries", "start")
                .WithOAuthBearerToken(tokenResponse.AccessToken)
                .PostJsonAsync(data)
                .ReceiveString();

            Console.WriteLine(result);
        }
    }
}
