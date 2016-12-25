namespace Yuki.Commands
{
    using System;

    using Req = InitializeRepositoryRequest;
    using Res = InitializeRepositoryResponse;

    public interface IInitializeRepositoryCommand
        : ICommand<Req, Res, Exception>
    {
    }
}
