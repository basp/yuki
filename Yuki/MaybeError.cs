namespace Yuki
{
    using System;

    public class Ok : IMaybeError
    {
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
    }

    public class Error : IMaybeError
    {
        private readonly Exception error;

        public Error(Exception ex = null)
        {
            this.error = ex;
        }

        public bool IsError
        {
            get
            {
                return true;
            }
        }

        public Exception Exception
        {
            get
            {
                return this.error;
            }
        }
    }

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
    }

    public class MaybeError<T>
    {
        private readonly T value;
        private readonly Exception error;

        public MaybeError(T value)
        {
            this.value = value;
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

        public bool IsError
        {
            get
            {
                return this.error != null;
            }
        }
    }
}
