namespace Yuki
{
    using System;

    public interface IMaybeError
    {
        bool IsError { get; }

        Exception Exception { get; }
    }

    public interface IMaybeError<T>
    {
        T Value { get; }

        Exception Exception { get; }
        
        bool IsError { get; }
    }
}
