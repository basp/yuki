namespace Yuki.Commands
{
    using System;
    using System.Diagnostics.Contracts;
    using NLog;
    using Optional;
    using Optional.Linq;

    using Req = MigrateRequest;
    using Res = MigrateResponse;

    public class MigrateCommand : IMigrateCommand
    {
        private readonly ILogger log = LogManager.GetCurrentClassLogger();

        private readonly ISessionFactory sessionFactory;
        private readonly Func<ISession, IGetVersionCommand> getVersionCommandFactory;
        private readonly Func<IResolveVersionCommand> resolveVersionCommandFactory;
        private readonly Func<ISession, IInsertVersionCommand> insertVersionCommandFactory;

        public MigrateCommand(
            ISessionFactory sessionFactory,
            Func<ISession, IGetVersionCommand> getVersionCommandFactory,
            Func<IResolveVersionCommand> resolveVersionCommandFactory,
            Func<ISession, IInsertVersionCommand> insertVersionCommandFactory)
        {
            Contract.Requires(sessionFactory != null);
            Contract.Requires(getVersionCommandFactory != null);
            Contract.Requires(resolveVersionCommandFactory != null);
            Contract.Requires(insertVersionCommandFactory != null);

            this.sessionFactory = sessionFactory;
            this.getVersionCommandFactory = getVersionCommandFactory;
            this.resolveVersionCommandFactory = resolveVersionCommandFactory;
            this.insertVersionCommandFactory = insertVersionCommandFactory;
        }

        public Option<Res, Exception> Execute(Req req)
        {
            using (var session = this.sessionFactory.Create())
            {
                session.Open();
                session.BeginTransaction();

                var res = from cv in this.GetCurrentVersion(session, req)
                          from nv in this.ResolveNextVersion(req)
                          from id in this.InsertNextVersion(session, req, nv.VersionName)
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

        private Option<InsertVersionResponse, Exception> InsertNextVersion(
            ISession session,
            Req req,
            string nextVersion)
        {
            var cmd = this.insertVersionCommandFactory(session);
            var res = cmd.Execute(new InsertVersionRequest
            {
                Server = req.Server,
                RepositoryDatabase = req.RepositoryDatabase,
                RepositorySchema = req.RepositorySchema,
                RepositoryPath = req.RepositoryPath,
                RepositoryVersion = nextVersion,
            });

            return res;
        }

        private Option<GetVersionResponse, Exception> GetCurrentVersion(
            ISession session,
            Req req)
        {
            var cmd = this.getVersionCommandFactory(session);
            var res = cmd.Execute(new GetVersionRequest
            {
                Server = req.Server,
                RepositoryDatabase = req.RepositoryDatabase,
                RepositorySchema = req.RepositorySchema,
                RepositoryPath = req.RepositoryPath,
            });

            return res;
        }

        private Option<ResolveVersionResponse, Exception> ResolveNextVersion(
            Req req)
        {
            var cmd = this.resolveVersionCommandFactory();
            var res = cmd.Execute(new ResolveVersionRequest
            {
                VersionFile = req.VersionFile,
            });

            return res;
        }
    }
}