namespace Yuki.Commands
{
    using System;
    using System.Diagnostics.Contracts;
    using Optional;
    using Optional.Linq;

    using Req = MigrateRequest;
    using Res = MigrateResponse;

    public class MigrateCommand : IMigrateCommand
    {
        private readonly ISessionFactory sessionFactory;
        private readonly Func<ISession, IGetVersionCommand> getVersionCommandFactory;
        private readonly Func<IResolveVersionCommand> resolveVersionCommandFactory;

        public MigrateCommand(
            ISessionFactory sessionFactory,
            Func<ISession, IGetVersionCommand> getVersionCommandFactory,
            Func<IResolveVersionCommand> resolveVersionCommandFactory)
        {
            Contract.Requires(sessionFactory != null);
            Contract.Requires(getVersionCommandFactory != null);
            Contract.Requires(resolveVersionCommandFactory != null);

            this.sessionFactory = sessionFactory;
            this.getVersionCommandFactory = getVersionCommandFactory;
            this.resolveVersionCommandFactory = resolveVersionCommandFactory;
        }

        public Option<Res, Exception> Execute(Req req)
        {
            using (var session = this.sessionFactory.Create())
            {
                var getVersionCmd = this.getVersionCommandFactory(session);
                var resolveVersionCmd = this.resolveVersionCommandFactory();

                session.Open();
                session.BeginTransaction();

                var currentVersion = getVersionCmd.Execute(new GetVersionRequest
                {
                    Server = req.Server,
                    RepositoryDatabase = req.RepositoryDatabase,
                    RepositorySchema = req.RepositorySchema,
                    RepositoryPath = req.RepositoryPath,
                });

                var nextVersion = resolveVersionCmd.Execute(new ResolveVersionRequest
                {
                    VersionFile = req.VersionFile,
                });

                return from cv in currentVersion
                       from nv in nextVersion
                       select CreateResponse(req, cv.VersionName, nv.VersionName);
            }
        }

        private static Res CreateResponse(
            Req req,
            string currentVersion,
            string newVersion)
        {
            return new Res
            {
                Server = req.Server,
                RepositoryPath = req.RepositoryPath,
                OldVersion = currentVersion,
                NewVersion = newVersion,
            };
        }
    }
}
