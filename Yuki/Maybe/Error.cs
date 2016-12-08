namespace Yuki.Maybe
{
    using System;

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
}
