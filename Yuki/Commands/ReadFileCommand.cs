namespace Yuki.Commands
{
    using System;
    using System.IO;
    using Optional;

    using static Optional.Option;

    using Req = ReadFileRequest;
    using Res = ReadFileResponse;

    public class ReadFileCommand : ICommand<Req, Res, Exception>
    {
        private readonly IHasher hasher;

        public ReadFileCommand(IHasher hasher)
        {
            this.hasher = hasher;
        }

        public Option<Res, Exception> Execute(Req request)
        {
            try
            {
                var contents = File.ReadAllText(request.Path);
                var hash = this.hasher.Hash(contents);
                return hash.Match(
                    some => Some<Res, Exception>(CreateResponse(contents, some, request)),
                    none => None<Res, Exception>(none));
            }
            catch (Exception ex)
            {
                var msg = $"Could not read file {request.Path}.";
                var error = new Exception(msg, ex);
                return None<Res, Exception>(error);
            }
        }

        private static Res CreateResponse(
            string contents,
            string hash,
            Req request)
        {
            return new Res()
            {
                Contents = contents,
                Path = request.Path,
                FileName = Path.GetFileName(request.Path),
                Hash = hash,
            };
        }
    }
}
