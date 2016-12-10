namespace Yuki
{
    using System;
    using System.Data.SqlClient;
    using Commands;
    using NLog;
    using NLog.Config;
    using NLog.Targets;
    using Optional;
    using PowerArgs;

    [ArgExceptionBehavior(ArgExceptionPolicy.StandardExceptionHandling)]
    internal class Program
    {
        private const string DefaultLoggingLayout = @"${pad:padding=5:inner=${level:uppercase=true}} ${date:format=HH\:mm\:ss} ${logger} ${message}";

        private readonly ILogger log = LogManager.GetCurrentClassLogger();

        [ArgActionMethod]
        public void CreateDatabase(CreateDatabaseRequest request)
        {
            var res = ExecuteDatabaseRequest(
                session => new CreateDatabase(session),
                request);

            res.MatchSome(x => this.log.Info(x));
            res.MatchNone(x => this.log.Error(x));
        }

        [ArgActionMethod]
        public void DropDatabase(DropDatabaseRequest request)
        {
            var res = ExecuteDatabaseRequest(
                session => new DropDatabase(session),
                request);

            res.MatchSome(x => this.log.Info(x));
            res.MatchNone(x => this.log.Error(x));
        }

        [ArgActionMethod]
        public void RestoreDatabase(RestoreDatabaseRequest request)
        {
            var res = ExecuteDatabaseRequest(
                session => new RestoreDatabase(session),
                request);

            res.MatchSome(x => this.log.Info(x));
            res.MatchNone(x => this.log.Error(x));
        }

        [ArgActionMethod]
        public void CreateRepository(CreateRepositoryRequest request)
        {
            var res = ExecuteDatabaseRequest(
                session => new CreateRepository(session),
                request);

            res.MatchSome(x => this.log.Info(x));
            res.MatchNone(x => this.log.Error(x));
        }

        private static Option<TResponse, TException> ExecuteDatabaseRequest<TRequest, TResponse, TException>(
            Func<ISession, ICommand<TRequest, TResponse, TException>> commandFactory,
            TRequest request)
            where TRequest : ISessionRequest
        {
            var sessionFactory = CreateSessionFactory(request.Server);
            using (var session = sessionFactory.Create())
            {
                session.Open();

                var cmd = commandFactory(session);
                return cmd.Execute(request);
            }
        }

        private static void InitializeLogManager()
        {
            var loggingConfig = new LoggingConfiguration();
            var target = new ColoredConsoleTarget()
            {
                Name = "console",
                Layout = DefaultLoggingLayout
            };

            var rule = new LoggingRule("*", LogLevel.Debug, target);

            loggingConfig.AddTarget(target);
            loggingConfig.LoggingRules.Add(rule);

            LogManager.Configuration = loggingConfig;
        }

        private static SqlConnectionStringBuilder CreateConnectionStringBuilder(string server) =>
             new SqlConnectionStringBuilder()
             {
                 ["Server"] = server,
                 ["Integrated Security"] = "SSPI"
             };

        private static ISessionFactory CreateSessionFactory(string server) =>
            new SqlSessionFactory(CreateConnectionStringBuilder(server));

        private static void Main(string[] args)
        {
            InitializeLogManager();

            Args.InvokeAction<Program>(args);
        }
    }
}
