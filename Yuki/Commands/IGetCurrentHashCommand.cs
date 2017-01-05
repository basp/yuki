namespace Yuki.Commands
{
    using System;

    using Req = GetCurrentHashRequest;
    using Res = GetCurrentHashResponse;

    public interface IGetCurrentHashCommand
        : ICommand<Req, Res, Exception>
    {
    }
}
