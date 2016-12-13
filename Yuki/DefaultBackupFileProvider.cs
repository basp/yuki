namespace Yuki
{
    using System;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Linq;
    using Optional;

    using static Optional.Option;

    public class DefaultBackupFileProvider : IBackupFileProvider
    {
        private readonly string folder;

        public DefaultBackupFileProvider(string folder)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(folder));

            this.folder = folder;
        }

        public Option<string, Exception> GetFullPath()
        {
            try
            {
                var backup = Directory.GetFiles(this.folder)
                   .Select(x => new FileInfo(x))
                   .OrderByDescending(x => x.LastWriteTime)
                   .Where(x => string.Equals(x.Extension, ".bak", StringComparison.InvariantCultureIgnoreCase))
                   .Select(x => x.FullName)
                   .First();

                return Some<string, Exception>(backup);
            }
            catch (Exception ex)
            {
                return None<string, Exception>(ex);
            }
        }
    }
}
