namespace Yuki.Maybe
{
    using System;

    public interface IMaybeError<T>
    {
        T Value { get; }

        Exception Exception { get; }

        bool IsError { get; }
    }
}
