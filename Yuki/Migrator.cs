namespace Yuki
{
    using System;
    using System.IO;
    using Maybe;
    using NLog;

    // TODO: 
    // Generalize so we can just specify a folder/name
    // and run the same thing for a single database.
    public class Migrator : IMigrator
    {
        private readonly ILogger log = LogManager.GetCurrentClassLogger();
        private readonly string databaseFolder;

        public Migrator(string databaseFolder)
        {
            this.databaseFolder = databaseFolder;
        }

        public IMaybeError ForEachDatabase(Action<string> action)
        {
            var cwd = Directory.GetCurrentDirectory();
            var path = Path.Combine(cwd, this.databaseFolder);

            if (!Directory.Exists(path))
            {
                var msg = $"Database folder {this.databaseFolder} doesn't exist.";
                var ex = new Exception(msg);
                return new MaybeError(ex);
            }

            var databases = Directory.GetDirectories(this.databaseFolder);
            Array.ForEach(databases, action);
            return new MaybeError();
        }
    }
}
