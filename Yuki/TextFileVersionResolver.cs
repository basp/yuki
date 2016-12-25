namespace Yuki
{
    using System;
    using System.Diagnostics.Contracts;
    using System.IO;
    using Optional;

    using static Optional.Option;

    public class TextFileVersionResolver : IVersionResolver
    {
        private readonly string path;

        public TextFileVersionResolver(string path)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(path));
            Contract.Requires(File.Exists(path));

            this.path = path;
        }

        public Option<string, Exception> Resolve()
        {
            try
            {
                var v = File.ReadAllText(this.path);
                return Some<string, Exception>(v);
            }
            catch (Exception ex)
            {
                return None<string, Exception>(ex);
            }
        }
    }
}