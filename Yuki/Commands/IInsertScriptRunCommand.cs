namespace Yuki.Commands
{
    using System;

    using Req = InsertScriptRunRequest;
    using Res = InsertScriptRunResponse;

    public interface IInsertScriptRunCommand
        : ICommand<Req, Res, Exception>
    {
    }
}
