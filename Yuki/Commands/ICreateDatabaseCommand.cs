namespace Yuki.Commands
{
    using System;

    using Req = CreateDatabaseRequest;
    using Res = CreateDatabaseResponse;

    public interface ICreateDatabaseCommand
        : ICommand<Req, Res, Exception>
    {
    }
}
