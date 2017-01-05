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
            // Don't cut off the directory seperator in case basePath already ends with it.
            var baseLen = Path.GetFullPath(basePath).EndsWith(Path.DirectorySeparatorChar.ToString())
                ? basePath.Length - 1
                : basePath.Length;

            return Path.GetFullPath(fullPath).Remove(0, baseLen);
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