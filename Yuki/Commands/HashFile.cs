namespace Yuki.Commands
{
    using System;
    using System.Diagnostics.Contracts;
    using System.IO;
    using Optional;

    using Req = HashRequest;
    using Res = HashResponse;

    public class HashFile : ICommand<Req, Res, Exception>
    {
        private readonly IHasher hasher;

        public HashFile(IHasher hasher)
        {
            Contract.Requires(hasher != null);

            this.hasher = hasher;
        }

        public Option<Res, Exception> Execute(Req request)
        {
            try
            {
                var value = File.ReadAllText(request.File);
                return this.hasher.Hash(value)
                    .Map(x => new Res(x));
            }
            catch (Exception ex)
            {
                return Option.None<Res, Exception>(ex);
            }
        }
    }
}
