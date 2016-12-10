namespace Yuki
{
    using System;
    using Optional;

    public interface IHasher
    {
        Option<string, Exception> Hash(string value);
    }
}
