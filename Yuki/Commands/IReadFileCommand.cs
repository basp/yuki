namespace Yuki.Commands
{
    using System;

    using Req = ReadFileRequest;
    using Res = ReadFileResponse;

    public interface IReadFileCommand
        : ICommand<Req, Res, Exception>
    {
    }
}
