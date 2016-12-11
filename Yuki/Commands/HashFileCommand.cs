namespace Yuki.Commands
{
    using System;
    using System.Diagnostics.Contracts;
    using System.IO;
    using Optional;

    using Req = HashFileRequest;
    using Res = HashFileResponse;

    public class HashFileCommand : ICommand<Req, Res, Exception>
    {
        private readonly IHasher hasher;

        public HashFileCommand(IHasher hasher)
        {
            Contract.Requires(hasher != null);

            this.hasher = hasher;
        }

        public Option<Res, Exception> Execute(Req req)
        {
            try
            {
                var value = File.ReadAllText(req.File);
                return this.hasher.Hash(value).Map(x => new Res(x));
            }
            catch (Exception ex)
            {
                return Option.None<Res, Exception>(ex);
            }
        }
    }
}
