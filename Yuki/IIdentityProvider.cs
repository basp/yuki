namespace Yuki
{
    using System;
    using Optional;

    public interface IIdentityProvider
    {
        Option<string, Exception> GetCurrent();
    }
}
