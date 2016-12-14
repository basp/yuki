namespace Yuki
{
    using System;
    using System.Diagnostics.Contracts;
    using System.IO;
    using NLog;
    using Optional;

    using static Optional.Option;

    public class TextFileVersionResolver : IVersionProvider
    {
        private readonly ILogger log = LogManager.GetCurrentClassLogger();

        private readonly string path;

        public TextFileVersionResolver(string path)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(path));

            this.path = path;
        }

        public Option<string, Exception> Resolve()
        {
            try
            {
                this.log.Info($"Attempting to resolve version from {this.path}");
                var version = File.ReadAllText(this.path).Trim();
                this.log.Info($"Got version {version} from {this.path}");
                return Some<string, Exception>(version);
            }
            catch (Exception ex)
            {
                return None<string, Exception>(ex);
            }
        }
    }
}
