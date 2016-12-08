namespace Yuki.Maybe
{
    using System;

    public class MaybeError : IMaybeError
    {
        private readonly Exception error;

        public MaybeError(Exception error = null)
        {
            this.error = error;
        }

        public Exception Exception
        {
            get
            {
                return this.error;
            }
        }

        public bool IsError
        {
            get
            {
                return this.error != null;
            }
        }

        public static IMaybeError<T> Create<T>(T value, Exception error = null)
        {
            return new MaybeError<T>(value, error);
        }
    }

    public class MaybeError<T> : IMaybeError<T>
    {
        private readonly T value;
        private readonly Exception error;

        public MaybeError(T value, Exception error = null)
        {
            this.value = value;
            this.error = error;
        }

        public MaybeError(Exception error)
        {
            this.error = error;
        }

        public T Value
        {
            get
            {
                return this.value;
            }
        }

        public Exception Exception
        {
            get
            {
                return this.error;
            }
        }

        public bool IsError
        {
            get
            {
                return this.error != null;
            }
        }
    }
}