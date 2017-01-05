namespace Yuki.Commands
{
    using System;
    using System.Diagnostics.Contracts;
    using System.IO;
    using NLog;
    using Optional;
    using Optional.Linq;

    using static Optional.Option;

    using Req = MigrateRequest;
    using Res = MigrateResponse;

    public class MigrateCommand : IMigrateCommand
    {
        private readonly ILogger log = LogManager.GetCurrentClassLogger();

        private readonly ISessionFactory sessionFactory;
        private readonly Func<ISession, Req, IMigrator> migratorFactory;

        public MigrateCommand(
            ISessionFactory sessionFactory,
            Func<ISession, Req, IMigrator> migratorFactory)
        {
            Contract.Requires(sessionFactory != null);
            Contract.Requires(migratorFactory != null);

            this.sessionFactory = sessionFactory;
            this.migratorFactory = migratorFactory;
        }

        public Option<Res, Exception> Execute(Req req)
        {
            using (var session = this.sessionFactory.Create())
            {
                session.Open();
                session.BeginTransaction();

                var migrator = this.migratorFactory(session, req);

                var res = from cv in migrator.GetCurrentVersion()
                          from nv in migrator.ResolveNextVersion()
                          from id in migrator.InsertNextVersion(cv.VersionName, nv.VersionName)
                          from r0 in migrator.RunMigrationScripts(
                              Path.Combine(req.ProjectFolder, "runBeforeUp"),
                              nv.VersionName,
                              id.VersionId,
                              false,
                              false)
                          from r1 in migrator.RunMigrationScripts(
                              Path.Combine(req.ProjectFolder, "up"),
                              nv.VersionName,
                              id.VersionId,
                              true,
                              false)
                          select CreateResponse(req, cv.VersionName, nv.VersionName, id.VersionId);

                res.MatchSome(x => session.CommitTransaction());
                res.MatchNone(x => session.RollbackTransaction());

                return res;
            }
        }

        private static Res CreateResponse(
            Req req,
            string currentVersion,
            string newVersion,
            int versionId)
        {
            return new Res
            {
                Server = req.Server,
                RepositoryPath = req.RepositoryPath,
                OldVersion = currentVersion,
                NewVersion = newVersion,
                VersionId = versionId,
            };
        }
    }
}