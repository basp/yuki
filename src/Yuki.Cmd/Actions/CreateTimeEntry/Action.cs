namespace Yuki.Cmd.Actions.CreateTimeEntry
{
    using System;
    using System.Collections.Generic;
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
            var tokenResponse = this.GetClientToken(this.tokenClient);

            var data = new Dictionary<string, object>
            {
                ["time_entry"] = new Dictionary<string, object>
                {
                    ["wid"] = args.Wid,
                    ["description"] = args.Description,
                    ["start"] = args.Start,
                    ["duration"] = args.Duration,
                    ["tid"] = args.Tid,
                    ["pid"] = args.Pid,
                },
            };

            var result = await Config.ApiEndPoint
                .AppendPathSegments("api", "time_entries")
                .WithOAuthBearerToken(tokenResponse.AccessToken)
                .PostJsonAsync(data)
                .ReceiveString();

            Console.WriteLine(result);
        }
    }
}
