namespace Yuki
{
    using System;
    using Maybe;

    public interface IMigrator
    {
        IMaybeError ForEachDatabase(Action<string> action);
    }
}
