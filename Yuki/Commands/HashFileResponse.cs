namespace Yuki.Commands
{
    using System.Diagnostics.Contracts;
    using Newtonsoft.Json;

    public class HashFileResponse
    {
        public HashFileResponse(string hash)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(hash));

            this.Hash = hash;
        }

        [JsonProperty(PropertyName = "hash")]
        public string Hash
        {
            get;
            set;
        }
    }
}
