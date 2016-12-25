namespace Yuki.Commands
{
    using System;

    using Req = ResolveVersionRequest;
    using Res = ResolveVersionResponse;

    public interface IResolveVersionCommand
        : ICommand<Req, Res, Exception>
    {
    }
}
