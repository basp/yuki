namespace Yuki.Commands
{
    using System;

    using Req = GetCurrentHashRequest;
    using Res = GetCurrentHashResponse;

    public interface IGetHashCommand
        : ICommand<Req, Res, Exception>
    {
    }
}
