namespace Yuki
{
    using System;
    using Optional;

    public interface IVersionResolver
    {
        Option<string, Exception> Resolve();
    }
}
