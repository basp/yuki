namespace Yuki.Commands
{
    using System.Diagnostics.Contracts;

    public class HashResponse
    {
        public HashResponse(string hash)
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
