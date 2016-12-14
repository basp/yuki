namespace Yuki.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
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

        public Option<Res, Exception> Execute(Req request)
        {
            try
            {
                var setupResult = this.setupDatabasesCmd.Execute(new WalkFoldersRequest()
                {
                    Folder = request.DatabasesFolder,
                });

                if (!setupResult.HasValue)
                {
                    return setupResult.Map(x => new Res());
                }

                var initRepoResult = this.createRepositoryCmd.Execute(new CreateRepositoryRequest()
                {
                    Server = request.Server,
                    Database = request.RepositoryDatabase,
                    Schema = request.RepositorySchema,
                });

                if (!initRepoResult.HasValue)
                {
                    return initRepoResult.Map(x => new Res());
                }

                return Some<Res, Exception>(new Res()
                {
                    Server = request.Server,
                    DatabaseFolder = request.DatabasesFolder,
                    RepositoryDatabase = request.RepositoryDatabase,
                    RepositorySchema = request.RepositorySchema,
                });
            }
            catch (Exception ex)
            {
                return None<Res, Exception>(ex);
            }
        }
    }
}
