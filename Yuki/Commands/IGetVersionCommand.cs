namespace Yuki.Commands
{
    using System;
    using Optional;

    using Req = GetVersionRequest;
    using Res = GetVersionResponse;

    public interface IGetVersionCommand
        : ICommand<Req, Res, Exception>
    {
    }
}
