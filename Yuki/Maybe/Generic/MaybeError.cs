namespace Yuki.Maybe
{
    using System;
    using Newtonsoft.Json;

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

        [JsonProperty(PropertyName = "value")]
        public T Value
        {
            get
            {
                return this.value;
            }
        }

        [JsonProperty(PropertyName = "exception")]
        public Exception Exception
        {
            get
            {
                return this.error;
            }
        }

        [JsonProperty(PropertyName = "isError")]
        public bool IsError
        {
            get
            {
                return this.error != null;
            }
        }
    }
}
