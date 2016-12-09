﻿namespace Yuki.Maybe
{
    using System;
    using Newtonsoft.Json;
  
    public class MaybeError : IMaybeError
    {
        private readonly Exception error;

        public MaybeError(Exception error = null)
        {
            this.error = error;
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

        public static IMaybeError<T> Create<T>(T value, Exception error = null)
        {
            return new MaybeError<T>(value, error);
        }
    }
}