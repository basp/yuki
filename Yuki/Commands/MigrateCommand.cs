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
                session.Open();
                session.BeginTransaction();

                var res = from cv in this.GetCurrentVersion(session, req)
                          from nv in this.ResolveNextVersion(req)
                          select CreateResponse(req, cv.VersionName, nv.VersionName);

                res.MatchSome(x => session.CommitTransaction());
                res.MatchNone(x => session.RollbackTransaction());

                return res;
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

        private Option<GetVersionResponse, Exception> GetCurrentVersion(
            ISession session,
            Req req)
        {
            var getVersionCmd = this.getVersionCommandFactory(session);
            return getVersionCmd.Execute(new GetVersionRequest
            {
                Server = req.Server,
                RepositoryDatabase = req.RepositoryDatabase,
                RepositorySchema = req.RepositorySchema,
                RepositoryPath = req.RepositoryPath,
            });
        }

        private Option<ResolveVersionResponse, Exception> ResolveNextVersion(
            Req req)
        {
            var resolveVersionCmd = this.resolveVersionCommandFactory();
            return resolveVersionCmd.Execute(new ResolveVersionRequest
            {
                VersionFile = req.VersionFile,
            });
        }
    }
}
