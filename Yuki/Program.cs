namespace Yuki
{
    using System;
    using System.Data.SqlClient;
    using System.IO;
    using AutoMapper;
    using Commands;
    using Optional;
    using Optional.Linq;
    using Serilog;

    using static Optional.Option;

    internal class Program
    {
        private static readonly ITextTemplateFactory TextTemplateFactory =
            new TextTemplateFactory();

        private static readonly ICommandFactory CommandFactory = new CommandFactory(
            new WindowsIdentityProvider(),
            new MD5Hasher(),
            TextTemplateFactory,
            session => new RepositoryFactory(session, TextTemplateFactory),
            session => new DatabaseFactory(session, TextTemplateFactory),
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
            Mapper.Initialize(cfg => cfg.AddProfile<MappingProfile>());

            Log.Logger = new LoggerConfiguration()
                .WriteTo.LiterateConsole()
                .CreateLogger();

            const string server = @"ROSPC0297\SQLEXPRESS";
            const string projectFolder = @"D:\temp\foo";

            Log.Information(
                "Project folder is {ProjectFolder} and target server is {Server}",
                projectFolder,
                server);

            var scriptFolders = new[]
            {
                new ScriptFolder(Path.Combine(projectFolder, "runBeforeUp"), false, false),
                new ScriptFolder(Path.Combine(projectFolder, "up"), true, false),
                new ScriptFolder(Path.Combine(projectFolder, "asdfaf"), false, false),
            };

            var setupCommand = new SetupCommand(
                 CreateSessionFactory(server),
                 () => DatabasesFolderProvider(projectFolder),
                 CommandFactory.CreateSetupDatabaseCommand,
                 CommandFactory.CreateInitializeRepositoryCommand);

            var migrateCommand = new MigrateCommand(
                CreateSessionFactory(server),
                scriptFolders,
                (session, req) => new Migrator(session, CommandFactory, req));

            var setupRequest = new SetupRequest
            {
                Server = server,
                Folder = projectFolder,
                RepositoryDatabase = Default.RepositoryDatabase,
                RepositorySchema = Default.RepositoryScheme,
                Restore = Default.Restore,
            };

            var migrateRequest = new MigrateRequest
            {
                Server = server,
                VersionFile = Path.Combine(projectFolder, Default.VersionFile),
                RepositoryDatabase = Default.RepositoryDatabase,
                RepositorySchema = Default.RepositoryScheme,
                RepositoryPath = "$/foo/bar/quux",
                ProjectFolder = projectFolder,
            };

            var res = from setupRes in setupCommand.Execute(setupRequest)
                      from migrateRes in migrateCommand.Execute(migrateRequest)
                      select new { Setup = setupRes, Migrate = migrateRes };

            res.MatchSome(x => Log.Information("Yay! I'm done!"));

            res.MatchNone(x => Log.Information(
                "Oh noes... Something went horribly wrong because bad {ExceptionType} thing :(",
                x.GetType()));

            res.MatchNone(x => Log.Error(x, x.Message));
        }
    }
}