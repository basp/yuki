namespace Yuki.Maybe
{
    using System;

    public class Result<T> : IMaybeError<T>
    {
        public Result(T value)
        {
            this.Value = value;
        }

        public Exception Exception
        {
            get
            {
                return null;
            }
        }

        public bool IsError
        {
            get
            {
                return false;
            }
        }

        public T Value
        {
            get;
            private set;
        }
    }
}