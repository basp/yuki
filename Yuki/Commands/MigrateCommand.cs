namespace Yuki.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Linq;
    using Optional;
    using Optional.Linq;

    using static Optional.Option;

    using Req = MigrateRequest;
    using Res = MigrateResponse;

    public class MigrateCommand : IMigrateCommand
    {
        private readonly ISessionFactory sessionFactory;
        private readonly IEnumerable<ScriptFolder> scriptFolders;
        private readonly Func<ISession, Req, IMigrator> migratorFactory;

        public MigrateCommand(
            ISessionFactory sessionFactory,
            IEnumerable<ScriptFolder> scriptFolders,
            Func<ISession, Req, IMigrator> migratorFactory)
        {
            Contract.Requires(sessionFactory != null);
            Contract.Requires(scriptFolders != null);
            Contract.Requires(migratorFactory != null);

            this.sessionFactory = sessionFactory;
            this.scriptFolders = scriptFolders;
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
                          from mr in this.RunMigrationFolders(
                              migrator,
                              req,
                              nv.VersionName,
                              id.VersionId)
                          select CreateResponse(
                              req,
                              cv.VersionName,
                              nv.VersionName,
                              id.VersionId,
                              mr);

                res.MatchSome(x => session.CommitTransaction());
                res.MatchNone(x => session.RollbackTransaction());

                return res;
            }
        }

        private static Res CreateResponse(
            Req req,
            string currentVersion,
            string newVersion,
            int versionId,
            IEnumerable<ScriptFolder> folders)
        {
            return new Res
            {
                Server = req.Server,
                RepositoryPath = req.RepositoryPath,
                OldVersion = currentVersion,
                NewVersion = newVersion,
                VersionId = versionId,
                Folders = folders.ToArray(),
            };
        }

        private Option<IEnumerable<ScriptFolder>, Exception> RunMigrationFolders(
            IMigrator migrator,
            Req req,
            string newVersion,
            int versionId)
        {
            foreach (var f in this.scriptFolders)
            {
                var res = migrator.RunMigrationScripts(
                     f.Path,
                     newVersion,
                     versionId,
                     f.IsOneTimeFolder,
                     f.IsEveryTimeFolder);

                if (!res.HasValue)
                {
                    return res.Map(x => Enumerable.Empty<ScriptFolder>());
                }
            }

            return Some<IEnumerable<ScriptFolder>, Exception>(this.scriptFolders);
        }
    }
}