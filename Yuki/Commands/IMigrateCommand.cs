namespace Yuki.Commands
{
    using System;

    using Req = MigrateRequest;
    using Res = MigrateResponse;

    public interface IMigrateCommand
        : ICommand<Req, Res, Exception>
    {
    }
}
