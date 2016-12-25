namespace Yuki.Commands
{
    using System;

    using Req = SetupDatabaseRequest;
    using Res = SetupDatabaseResponse;

    public interface ISetupDatabaseCommand : ICommand<Req, Res, Exception>
    {
    }
}
