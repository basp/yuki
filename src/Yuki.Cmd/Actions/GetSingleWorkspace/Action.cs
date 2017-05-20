namespace Yuki.Cmd.Actions.GetSingleWorkspace
{
    using System;
    using System.Threading.Tasks;
    using IdentityModel.Client;

    public class Action : IAction<Args>
    {
        private readonly TokenClient tokenClient;

        public Action(TokenClient tokenClient)
        {
            this.tokenClient = tokenClient;
        }

        public Task Execute(Args args)
        {
            throw new NotImplementedException();
        }
    }
}
