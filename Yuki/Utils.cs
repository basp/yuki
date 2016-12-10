namespace Yuki
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Reflection;

    public static class Utils
    {
        public static KeyValuePair<TKey, TValue> CreateKeyValuePair<TKey, TValue>(TKey key, TValue value) => 
            new KeyValuePair<TKey, TValue>(key, value);

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
