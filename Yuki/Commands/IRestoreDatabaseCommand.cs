namespace Yuki.Commands
{
    using System;

    using Req = RestoreDatabaseRequest;
    using Res = RestoreDatabaseResponse;

    public interface IRestoreDatabaseCommand
        : ICommand<Req, Res, Exception>
    {
    }
}