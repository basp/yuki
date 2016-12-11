
namespace Yuki
{
    using System;
    using System.Collections.Generic;
    using Optional;
    using PowerArgs;
    using Newtonsoft.Json;

    public static class Revivers
    {
        [ArgReviver]
        public static Option<IDictionary<string, object>, Exception> JsonReviver(string name, string value)
        {
            try
            {
                var obj = JsonConvert.DeserializeObject<IDictionary<string, object>>(value);
                return Option.Some<IDictionary<string, object>, Exception>(obj);
            }
            catch (Exception ex)
            {
                return Option.None<IDictionary<string, object>, Exception>(ex);
            }
        }
    }
}
