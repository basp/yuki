namespace Yuki.Maybe
{
    using System;

    public class Nothing : IMaybeError
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
}
