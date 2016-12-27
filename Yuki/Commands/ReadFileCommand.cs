namespace Yuki.Commands
{
    using System;
    using System.Diagnostics.Contracts;
    using System.IO;
    using Optional;
    using Optional.Linq;

    using static Optional.Option;

    using Req = ReadFileRequest;
    using Res = ReadFileResponse;

    public class ReadFileCommand : IReadFileCommand
    {
        private readonly IHasher hasher;

        public ReadFileCommand(IHasher hasher)
        {
            Contract.Requires(hasher != null);

            this.hasher = hasher;
        }

        public Option<Res, Exception> Execute(Req req)
        {
            return from text in ReadAllText(req.Path)
                   from hash in this.hasher.GetHash(text)
                   select CreateResponse(req, text, hash);
        }

        private static Res CreateResponse(
            Req req,
            string text,
            string hash)
        {
            return new Res
            {
                Text = text,
                Hash = hash,
            };
        }

        private static Option<string, Exception> ReadAllText(string path)
        {
            try
            {
                var text = File.ReadAllText(path);
                return Some<string, Exception>(text);
            }
            catch (Exception ex)
            {
                return None<string, Exception>(ex);
            }
        }
    }
}
