namespace Yuki.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.IO;
    using NLog;
    using Optional;

    using static Optional.Option;

    using Req = SetupRequest;
    using Res = SetupResponse;

    public class SetupCommand<TWalkRes> : ICommand<Req, Res, Exception>
    {
        private readonly ILogger log = LogManager.GetCurrentClassLogger();

        private readonly ISession session;
        private readonly IBackupFileProvider backupFileProvider;
        private readonly ICommand<WalkFoldersRequest, IList<TWalkRes>, Exception> setupDatabasesCmd;
        private readonly ICommand<CreateRepositoryRequest, CreateRepositoryResponse, Exception> createRepositoryCmd;

        public SetupCommand(
            ISession session,
            IBackupFileProvider backupFileProvider,
            ICommand<WalkFoldersRequest, IList<TWalkRes>, Exception> setupDatabasesCmd,
            ICommand<CreateRepositoryRequest, CreateRepositoryResponse, Exception> createRepositoryCmd)
        {
            Contract.Requires(session != null);
            Contract.Requires(backupFileProvider != null);
            Contract.Requires(setupDatabasesCmd != null);
            Contract.Requires(createRepositoryCmd != null);

            this.session = session;
            this.backupFileProvider = backupFileProvider;
            this.setupDatabasesCmd = setupDatabasesCmd;
            this.createRepositoryCmd = createRepositoryCmd;
        }

        public Option<Res, Exception> Execute(Req req)
        {
            try
            {
                var walkFoldersRequest = new WalkFoldersRequest()
                {
                    Folder = req.DatabasesFolder,
                };

                var initRepoRequest = new CreateRepositoryRequest()
                {
                    Server = req.Server,
                    RepositoryDatabase = req.RepositoryDatabase,
                    RepositorySchema = req.RepositorySchema,
                };

                return this.setupDatabasesCmd
                    .Execute(walkFoldersRequest)
                    .FlatMap(x => this.createRepositoryCmd.Execute(initRepoRequest))
                    .Map(x => CreateResponse(req));
            }
            catch (Exception ex)
            {
                return None<Res, Exception>(ex);
            }
        }

        private static Res CreateResponse(Req req)
        {
            return new Res()
            {
                Server = req.Server,
                DatabaseFolder = Path.GetFullPath(req.DatabasesFolder),
                RepositoryDatabase = req.RepositoryDatabase,
                RepositorySchema = req.RepositorySchema,
            };
        }
    }
}