namespace Yuki
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.IO;
    using Commands;
    using Newtonsoft.Json;
    using NLog;
    using Optional;

    using static Optional.Option;
    using static System.Console;

    internal class Program
    {
        private static ILogger log = LogManager.GetCurrentClassLogger();

        private static SqlConnectionStringBuilder CreateConnectionStringBuilder(string server) =>
             new SqlConnectionStringBuilder()
             {
                 ["Server"] = server,
                 ["Integrated Security"] = "SSPI",
             };

        private static ISessionFactory CreateSessionFactory(string server) =>
            new SqlSessionFactory(CreateConnectionStringBuilder(server));

        private static void Main(string[] args)
        {
            const string server = @"ROSPC0297\SQLEXPRESS";

            Func<ISession, ISetupDatabaseCommand> setupDatabaseCommandFactory =
                session => new SetupDatabaseCommand(
                    new CreateDatabaseCommand(session),
                    new RestoreDatabaseCommand(session));

            Func<ISession, IInitializeRepositoryCommand> initializeRepositoryCommandFactory =
                session => new InitializeRepositoryCommand(session);

            var setupCommand = new SetupCommand(
                CreateSessionFactory(server),
                () => Some<string, Exception>(@"D:\temp\foo\databases"),
                setupDatabaseCommandFactory,
                initializeRepositoryCommandFactory);

            var setupRequest = new SetupRequest
            {
                Server = server,
                Folder = @"D:\temp\foo",
                RepositoryDatabase = "yuki",
                RepositorySchema = "dbo",
            };

            var setupResult = setupCommand.Execute(setupRequest);

            setupResult.MatchSome(x => WriteLine(JsonConvert.SerializeObject(x)));
            setupResult.MatchNone(x => WriteLine(x.ToString()));
        }
    }
}