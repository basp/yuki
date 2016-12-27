namespace Yuki.Commands
{
    using System;

    using Req = InsertScriptRunErrorRequest;
    using Res = InsertScriptRunErrorResponse;

    public interface IInsertScriptRunErrorCommand
        : ICommand<Req, Res, Exception>
    {
    }
}
