namespace Yuki
{
    using System;
    using Optional;

    public interface IVersionProvider
    {
        Option<string, Exception> Resolve();
    }
}