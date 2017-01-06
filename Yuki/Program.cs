namespace Yuki
{
    using System;
    using System.Data.SqlClient;
    using System.IO;
    using Commands;
    using Optional;
    using Optional.Linq;
    using Serilog;

    using static Optional.Option;

    internal class Program
    {
        private static readonly ICommandFactory CommandFactory = new CommandFactory(
            new WindowsIdentityProvider(),
            new MD5Hasher(),
            path => new TextFileVersionResolver(path));

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
            Log.Logger = new LoggerConfiguration()
                .WriteTo.LiterateConsole()
                .CreateLogger();

            const string server = @"ROSPC0297\SQLEXPRESS";
            const string folder = @"D:\temp\foo";

            Log.Information(
                "Project folder is {ProjectFolder} and target server is {Server}",
                folder,
                server);

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
                (session, req) => new Migrator(session, CommandFactory, req));

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

            Log.Information("Done!");
        }
    }
}