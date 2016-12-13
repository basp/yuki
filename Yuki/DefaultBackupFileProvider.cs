namespace Yuki
{
    using System;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Linq;
    using NLog;
    using Optional;

    using static Optional.Option;

    public class DefaultBackupFileProvider : IBackupFileProvider
    {
        private readonly ILogger log = LogManager.GetCurrentClassLogger();

        private readonly Func<FileInfo, IComparable> ordering;

        public DefaultBackupFileProvider()
            : this(x => x.LastWriteTime)
        {
        }

        public DefaultBackupFileProvider(Func<FileInfo, IComparable> ordering)
        {
            Contract.Requires(ordering != null);

            this.ordering = ordering;
        }

        public Option<string, Exception> TryFindIn(string folder)
        {
            try
            {
                this.log.Debug($"Looking for backup files in folder {folder}");

                var files = Directory
                    .GetFiles(folder, "*.bak", SearchOption.TopDirectoryOnly)
                    .Select(x => new FileInfo(x))
                    .ToArray();

                Array.ForEach(files, x => this.log.Debug($"> {x.FullName}"));
                var backup = files
                   .OrderByDescending(this.ordering)
                   .Select(x => x.FullName)
                   .First();

                this.log.Debug($"Found backup candidate {backup}");
                return Some<string, Exception>(backup);
            }
            catch (Exception ex)
            {
                return None<string, Exception>(ex);
            }
        }
    }
}
