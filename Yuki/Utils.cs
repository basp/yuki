namespace Yuki
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Reflection;
    using Optional;
    using static Optional.Option;

    public static class Utils
    {
        public static KeyValuePair<TKey, TValue> CreateKeyValuePair<TKey, TValue>(TKey key, TValue value) => 
            new KeyValuePair<TKey, TValue>(key, value);

        public static Option<object> MaybeInt(string value)
        {
            int i;
            if (int.TryParse(value, out i))
            {
                return Some<object>(i);
            }

            return None<object>();
        }

        public static Option<object> MaybeDecimal(string value)
        {
            decimal d;
            if (decimal.TryParse(value, out d))
            {
                return Some<object>(d);
            }

            return None<object>();
        }

        public static Option<object> MaybeDateTime(string value)
        {
            DateTime dt;
            if (DateTime.TryParse(value, out dt))
            {
                return Some<object>(dt);
            }

            return None<object>();
        }

        public static string ReadEmbeddedString(this Assembly asm, string resourceName)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(resourceName));

            using (var stream = asm.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    var msg = $"Could not find resource {resourceName} in assembly {asm.FullName}";
                    throw new ArgumentException(msg, nameof(resourceName));
                }

                var reader = new StreamReader(stream);
                return reader.ReadToEnd();
            }
        }
    }
}
