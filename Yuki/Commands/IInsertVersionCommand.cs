namespace Yuki.Commands
{
    using System;

    using Req = InsertVersionRequest;
    using Res = InsertVersionResponse;

    public interface IInsertVersionCommand
        : ICommand<Req, Res, Exception>
    {
    }
}
