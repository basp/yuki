namespace Yuki.Commands
{
    using System;

    using Req = RunScriptsRequest;
    using Res = RunScriptsResponse;

    public interface IRunScriptsCommand
        : ICommand<Req, Res, Exception>
    {
    }
}
