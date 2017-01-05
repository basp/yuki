namespace Yuki
{
    using System;
    using System.Data.SqlClient;
    using System.IO;
    using Commands;
    using Newtonsoft.Json;
    using NLog;
    using Optional;
    using Optional.Linq;

    using static Optional.Option;
    using static System.Console;

    internal class Program
    {
        private static readonly ICommandFactory CommandFactory = new CommandFactory(
            new WindowsIdentityProvider(),
            new MD5Hasher(),
            path => new TextFileVersionResolver(path));

        private ILogger log = LogManager.GetCurrentClassLogger();

        private static SqlConnectionStringBuilder CreateConnectionStringBuilder(string server) =>
             new SqlConnectionStringBuilder()
             {
                 ["Server"] = server,
                 ["Integrated Security"] = "SSPI",
             };

        private static ISessionFactory CreateSessionFactory(string server) =>
            new SqlSessionFactory(CreateConnectionStringBuilder(server));

        private static Option<string, Exception> DatabasesFolderProvider(string cwd) =>
            Some<string, Exception>(Path.Combine(cwd, Default.DatabasesFolder));

        private static void Main(string[] args)
        {
            const string server = @"ROSPC0297\SQLEXPRESS";
            const string folder = @"D:\temp\foo";

            var cwd = Directory.GetCurrentDirectory();

            var setupCommand = new SetupCommand(
                 CreateSessionFactory(server),
                 () => DatabasesFolderProvider(folder),
                 CommandFactory.CreateSetupDatabaseCommand,
                 CommandFactory.CreateInitializeRepositoryCommand);

            var commandFactory = new CommandFactory(
                new WindowsIdentityProvider(),
                new MD5Hasher(),
                x => new TextFileVersionResolver(x));

            var migrateCommand = new MigrateCommand(
                CreateSessionFactory(server),
                (session, req) => new Migrator(
                    session,
                    CommandFactory,
                    req));

            var setupRequest = new SetupRequest
            {
                Server = server,
                Folder = folder,
                RepositoryDatabase = Default.RepositoryDatabase,
                RepositorySchema = Default.RepositoryScheme,
                Restore = Default.Restore,
            };

            var migrateRequest = new MigrateRequest
            {
                Server = server,
                VersionFile = Path.Combine(folder, Default.VersionFile),
                RepositoryDatabase = Default.RepositoryDatabase,
                RepositorySchema = Default.RepositoryScheme,
                RepositoryPath = "$/foo/bar/quux",
                ProjectFolder = folder,
            };

            var res = from setupRes in setupCommand.Execute(setupRequest)
                      from migrateRes in migrateCommand.Execute(migrateRequest)
                      select new { Setup = setupRes, Migrate = migrateRes };

            res.MatchSome(x => WriteLine(JsonConvert.SerializeObject(x, Formatting.Indented)));
            res.MatchNone(x => WriteLine(x.ToString()));
        }
    }
}