
namespace Yuki
{
    using System;
    using System.Collections.Generic;
    using Optional;
    using PowerArgs;

    public static class Revivers
    {
        [ArgReviver]
        public static Option<IDictionary<string, string>, Exception> StringPairReviver(
          string name,
          string value)
        {
            var msg = $"Could not parse parameter '{name}' with argument '{value}'.";
            var error = new Exception(msg);
            return Option.None<IDictionary<string, string>, Exception>(error);
        }

        [ArgReviver]
        public static Option<IDictionary<string, object>, Exception> KeyValuePairReviver(
            string name,
            string value)
        {
            var msg = $"Could not parse parameter '{name}' with argument '{value}'.";
            var error = new Exception(msg);
            return Option.None<IDictionary<string, object>, Exception>(error);
        }
    }
}
