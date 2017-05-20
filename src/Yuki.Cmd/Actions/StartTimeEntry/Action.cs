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
            var data = new Dictionary<string, object>
            {
                ["wid"] = args.WorkspaceId,
                ["description"] = args.Description,
            };

            var result = await Config.Server
                .AppendPathSegments("api", "time_entries", "start")
                .PostJsonAsync(data)
                .ReceiveString();

            Console.WriteLine(result);
        }
    }
}
