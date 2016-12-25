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
        public static Option<string, Exception> ReadEmbeddedString(this Assembly asm, string resourceName)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(resourceName));

            using (var stream = asm.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    var msg = $"Could not find resource {resourceName} in assembly {asm.FullName}";
                    var ex = new ArgumentException(msg, "resourceName");
                    return None<string, Exception>(ex);
                }

                var reader = new StreamReader(stream);
                var str = reader.ReadToEnd();
                return Some<string, Exception>(str);
            }
        }
    }
}