namespace Yuki.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Linq;
    using NLog;
    using Optional;
    using Optional.Linq;

    using static Optional.Option;

    using Req = SetupRequest;
    using Res = SetupResponse;

    public class SetupCommand : ISetupCommand
    {
        private readonly ILogger log = LogManager.GetCurrentClassLogger();

        private readonly ISessionFactory sessionFactory;
        private readonly Func<Option<string, Exception>> databasesFolderProvider;
        private readonly Func<ISession, ISetupDatabaseCommand> setupDatabaseCommandFactory;
        private readonly Func<ISession, IInitializeRepositoryCommand> initializeRepositoryCommandFactory;

        public SetupCommand(
            ISessionFactory sessionFactory,
            Func<Option<string, Exception>> databasesFolderProvider,
            Func<ISession, ISetupDatabaseCommand> setupDatabaseCommandFactory,
            Func<ISession, IInitializeRepositoryCommand> initializeRepositoryCommandFactory)
        {
            Contract.Requires(sessionFactory != null);
            Contract.Requires(databasesFolderProvider != null);
            Contract.Requires(setupDatabaseCommandFactory != null);
            Contract.Requires(initializeRepositoryCommandFactory != null);

            this.sessionFactory = sessionFactory;
            this.databasesFolderProvider = databasesFolderProvider;
            this.setupDatabaseCommandFactory = setupDatabaseCommandFactory;
            this.initializeRepositoryCommandFactory = initializeRepositoryCommandFactory;
        }

        public Option<Res, Exception> Execute(Req req)
        {
            using (var session = this.sessionFactory.Create())
            {
                session.Open();

                return from databasesFolder in this.databasesFolderProvider()
                       from setupDatabaseResponses in this.SetupDatabases(
                           session,
                           req.Server,
                           databasesFolder,
                           req.Restore)
                       from initializeRepositoryResponse in this.InitializeRepository(
                           session,
                           req.Server,
                           req.RepositoryDatabase,
                           req.RepositorySchema)
                       select CreateResponse(
                           req,
                           setupDatabaseResponses,
                           initializeRepositoryResponse);
            }
        }

        private static Res CreateResponse(
            Req req,
            SetupDatabaseResponse[] setupDatabaseResponses,
            InitializeRepositoryResponse initializeRepositoryResponse)
        {
            var repository = new
            {
                initializeRepositoryResponse.RepositoryDatabase,
                initializeRepositoryResponse.RepositorySchema,
            };

            var databases = setupDatabaseResponses.Select(x => new
            {
                x.Database,
                x.Created,
                x.Folder,
                x.Backup,
                x.Restored,
            });

            return new Res
            {
                Databases = databases.ToArray(),
                Repository = repository,
            };
        }

        private Option<InitializeRepositoryResponse, Exception> InitializeRepository(
            ISession session,
            string server,
            string repositoryDatabase,
            string repositorySchema)
        {
            var req = new InitializeRepositoryRequest
            {
                Server = server,
                RepositoryDatabase = repositoryDatabase,
                RepositorySchema = repositorySchema,
            };

            var cmd = this.initializeRepositoryCommandFactory(session);
            return cmd.Execute(req);
        }

        private Option<SetupDatabaseResponse[], Exception> SetupDatabases(
            ISession session,
            string server,
            string databasesFolder,
            bool restore = false)
        {
            var folders = Directory.GetDirectories(databasesFolder);
            var results = new List<SetupDatabaseResponse>();
            foreach (var f in folders)
            {
                var req = new SetupDatabaseRequest
                {
                    Server = server,
                    Folder = f,
                    Restore = restore,
                };

                var cmd = this.setupDatabaseCommandFactory(session);
                var res = cmd.Execute(req);

                if (!res.HasValue)
                {
                    return res.Map(x => new SetupDatabaseResponse[0]);
                }

                res.MatchSome(x => results.Add(x));
            }

            return Some<SetupDatabaseResponse[], Exception>(results.ToArray());
        }
    }
}
