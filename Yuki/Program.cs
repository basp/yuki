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
        private ILogger log = LogManager.GetCurrentClassLogger();

        private static SqlConnectionStringBuilder CreateConnectionStringBuilder(string server) =>
             new SqlConnectionStringBuilder()
             {
                 ["Server"] = server,
                 ["Integrated Security"] = "SSPI",
             };

        private static IReadFileCommand CreateReadFileCommand() =>
            new ReadFileCommand(new MD5Hasher());

        private static ISessionFactory CreateSessionFactory(string server) =>
            new SqlSessionFactory(CreateConnectionStringBuilder(server));

        private static ISetupDatabaseCommand CreateSetupDatabaseCommand(ISession session) =>
            new SetupDatabaseCommand(
                new CreateDatabaseCommand(session),
                new RestoreDatabaseCommand(session));

        private static IInitializeRepositoryCommand CreateInitializeRepositoryCommand(ISession session) =>
                new InitializeRepositoryCommand(session);

        private static IGetVersionCommand CreateGetVersionCommand(ISession session) =>
            new GetVersionCommand(session);

        private static IResolveVersionCommand CreateResolveVersionCommand(
            Func<string, IVersionResolver> resolverFactory) =>
                new ResolveVersionCommand(resolverFactory);

        private static IInsertVersionCommand CreateInsertVersionCommand(ISession session) =>
            new InsertVersionCommand(session, new WindowsIdentityProvider());

        private static IRunScriptsCommand CreateRunScriptsCommand(ISession session) =>
            new RunScriptsCommand(
                session,
                CreateReadFileCommand(),
                CreateHasScriptRunCommand(session),
                CreateGetCurrentHashcommand(session));

        private static IGetCurrentHashCommand CreateGetCurrentHashcommand(ISession session) =>
            new GetCurrentHashCommand(session);

        private static IHasScriptRunCommand CreateHasScriptRunCommand(ISession session) =>
            new HasScriptRunCommand(session);

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
                 CreateSetupDatabaseCommand,
                 CreateInitializeRepositoryCommand);

            var migrateCommand = new MigrateCommand(
                CreateSessionFactory(server),
                CreateGetVersionCommand,
                () => CreateResolveVersionCommand(x => new TextFileVersionResolver(x)),
                CreateInsertVersionCommand,
                CreateRunScriptsCommand);

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