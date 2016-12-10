namespace Yuki.Commands
{
    using System.Diagnostics.Contracts;

    public class HashFileResponse
    {
        public HashFileResponse(string hash)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(hash));

            this.Hash = hash;
        }

        public string Hash
        {
            get;
            set;
        }
    }
}
