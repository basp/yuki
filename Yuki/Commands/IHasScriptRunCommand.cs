namespace Yuki.Commands
{
    using System;

    using Req = HasScriptRunRequest;
    using Res = HasScriptRunResponse;

    public interface IHasScriptRunCommand
        : ICommand<Req, Res, Exception>
    {
    }
}
