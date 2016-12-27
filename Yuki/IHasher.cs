namespace Yuki
{
    using System;
    using Optional;

    public interface IHasher
    {
        Option<string, Exception> GetHash(string value);
    }
}
