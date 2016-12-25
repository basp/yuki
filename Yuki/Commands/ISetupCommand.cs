namespace Yuki.Commands
{
    using System;

    using Req = SetupRequest;
    using Res = SetupResponse;

    public interface ISetupCommand : ICommand<Req, Res, Exception>
    {
    }
}
