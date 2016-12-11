namespace Yuki
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    using Optional;

    using static Optional.Option;

    public class MD5Hasher : IHasher
    {
        private readonly MD5 provider = MD5.Create();

        public Option<string, Exception> Hash(string value)
        {
            try
            {
                var bytes = Encoding.UTF8.GetBytes(value);
                var hash = this.provider.ComputeHash(bytes);
                var str = Convert.ToBase64String(hash);
                return Some<string, Exception>(str);
            }
            catch (Exception ex)
            {
                return None<string, Exception>(ex);
            }
        }
    }
}
