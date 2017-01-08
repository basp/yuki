namespace Yuki
{
    using System;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Reflection;
    using Optional;

    using static Optional.Option;

    public static class Utils
    {
        public static string FullyQualifiedObjectName(
            string database,
            string schema,
            string obj) => $"[{database}].[{schema}].{obj}";

        public static string RelativePath(string basePath, string fullPath)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(basePath));
            Contract.Requires(!string.IsNullOrWhiteSpace(fullPath));

            if (!basePath.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                basePath += Path.DirectorySeparatorChar.ToString();
            }

            var fullUri = new Uri(fullPath);
            var baseUri = new Uri(basePath);
            var relativeUri = baseUri.MakeRelativeUri(fullUri);

            // There's no predefined `UrlSeperatorChar` because it's defined
            // in the spec as '/' and only '/' on every system.
            return relativeUri.ToString().Replace('/', Path.DirectorySeparatorChar);
        }

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