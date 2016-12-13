namespace Yuki
{
    using System;
    using System.Data.SqlClient;
    using Commands;
    using Newtonsoft.Json;
    using NLog;
    using NLog.Config;
    using NLog.Targets;
    using Optional;
    using PowerArgs;

    using static Optional.Option;
    using static System.Console;

    [ArgExceptionBehavior(ArgExceptionPolicy.StandardExceptionHandling)]
    internal class Program
    {
        private const string DefaultLoggingLayout =
            @"${pad:padding=5:inner=${level:uppercase=true}} ${date:format=HH\:mm\:ss} ${logger} ${message}";

        private readonly ILogger log = LogManager.GetCurrentClassLogger();

        [HelpHook]
        [ArgShortcut("-?")]
        public bool Help
        {
            get;
            set;
        }

        [ArgActionMethod]
        public void CreateDatabase(CreateDatabaseRequest request)
        {
            var res = ExecuteDatabaseRequest(
                session => new CreateDatabaseCommand(session),
                request);

            res.MatchSome(x => WriteLine(JsonConvert.SerializeObject(x)));
            res.MatchNone(x => this.log.Error(x));
        }

        [ArgActionMethod]
        public void DropDatabase(DropDatabaseRequest request)
        {
            var res = ExecuteDatabaseRequest(
                session => new DropDatabaseCommand(session),
                request);

            res.MatchSome(x => WriteLine(JsonConvert.SerializeObject(x)));
            res.MatchNone(x => this.log.Error(x));
        }

        [ArgActionMethod]
        public void RestoreDatabase(RestoreDatabaseRequest request)
        {
            var res = ExecuteDatabaseRequest(
                session => new RestoreDatabaseCommand(session),
                request);

            res.MatchSome(x => this.log.Info(x));
            res.MatchNone(x => this.log.Error(x));
        }

        [ArgActionMethod]
        public void CreateRepository(CreateRepositoryRequest request)
        {
            var res = ExecuteDatabaseRequest(
                session => new CreateRepositoryCommand(session),
                request);

            res.MatchSome(x => this.log.Info(x));
            res.MatchNone(x => this.log.Error(x));
        }

        [ArgActionMethod]
        public void InsertVersion(InsertVersionRequest request)
        {
            var identityProvider = new WindowsIdentityProvider();
            var res = ExecuteDatabaseRequest(
                session => new InsertVersionCommand(session, identityProvider),
                request);

            res.MatchSome(x => WriteLine(JsonConvert.SerializeObject(x)));
            res.MatchNone(x => this.log.Error(x));
        }

        [ArgActionMethod]
        public void GetVersion(GetVersionRequest request)
        {
            var res = ExecuteDatabaseRequest(
                session => new GetVersionCommand(session),
                request);

            res.MatchSome(x => WriteLine(JsonConvert.SerializeObject(x)));
            res.MatchNone(x => this.log.Error(x));
        }

        [ArgActionMethod]
        public void GetHash(GetHashRequest request)
        {
            throw new NotImplementedException();
        }

        [ArgActionMethod]
        public void HashFile(HashFileRequest request)
        {
            var hasher = new MD5Hasher();
            var cmd = new HashFileCommand(hasher);
            var res = cmd.Execute(request);

            res.MatchSome(x => WriteLine(x.Hash));
            res.MatchNone(x => this.log.Error(x));
        }

        [ArgActionMethod]
        public void ReadFile(ReadFileRequest request)
        {
            var hasher = new MD5Hasher();
            var cmd = new ReadFileCommand(hasher);
            var res = cmd.Execute(request);

            res.MatchSome(x => WriteLine(JsonConvert.SerializeObject(x)));
            res.MatchNone(x => this.log.Error(x));
        }

        [ArgActionMethod]
        public void WalkFolders(WalkFoldersRequest request)
        {
            Func<string, Option<WalkFoldersResponse, Exception>> walker = dir =>
            {
                this.log.Info(dir);
                return Some<WalkFoldersResponse, Exception>(new WalkFoldersResponse());
            };

            var cmd = new WalkFoldersCommand<WalkFoldersResponse>(walker);
            var res = cmd.Execute(request);

            res.MatchSome(x => this.log.Info(x));
            res.MatchNone(x => this.log.Error(x));
        }

        [ArgActionMethod]
        public void Setup(SetupRequest request)
        {
            var sessionFactory = CreateSessionFactory(request.Server);
            using (var session = sessionFactory.Create())
            {
                session.Open();

                var backupFileProvider = new DefaultBackupFileProvider();

                var createDatabaseCmd = new CreateDatabaseCommand(
                    session);

                var restoreDatabaseCmd = new RestoreDatabaseCommand(
                    session);

                var setupDatabaseCmd = new SetupDatabaseCommand(
                    session,
                    backupFileProvider,
                    createDatabaseCmd,
                    restoreDatabaseCmd);

                var walkFoldersCmd = new WalkFoldersCommand<object>(
                    new Func<string, Option<object, Exception>>(folder =>
                    {
                        var setupDatabaseRequest = new SetupDatabaseRequest()
                        {
                            Server = request.Server,
                            Restore = request.Restore,
                            RestoreTimeout = request.RestoreTimeout,
                            Folder = folder,
                        };

                        var setupDatabaseResult = setupDatabaseCmd.Execute(setupDatabaseRequest);
                        return setupDatabaseResult.Map(x => (object)x);
                    }));

                var walkFoldersRequest = new WalkFoldersRequest()
                {
                    Folder = request.Folder,
                };

                var res = walkFoldersCmd.Execute(walkFoldersRequest);

                res.MatchSome(x => WriteLine(JsonConvert.SerializeObject(x)));
                res.MatchNone(x => this.log.Error(x));
            }
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
                Layout = DefaultLoggingLayout,
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
                 ["Integrated Security"] = "SSPI",
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