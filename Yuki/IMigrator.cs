namespace Yuki
{
    using System;

    public interface IMigrator
    {
        IMaybeError ForEachDatabase(Action<string> action);
    }
}
