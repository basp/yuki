namespace Yuki.Commands
{
    using System;
    using System.Diagnostics.Contracts;
    using NLog;
    using Optional;
    using Optional.Linq;

    using Req = ResolveVersionRequest;
    using Res = ResolveVersionResponse;

    public class ResolveVersionCommand
        : IResolveVersionCommand
    {
        private readonly ILogger log = LogManager.GetCurrentClassLogger();

        private readonly Func<string, IVersionResolver> resolverFactory;

        public ResolveVersionCommand(
            Func<string, IVersionResolver> resolverFactory)
        {
            Contract.Requires(resolverFactory != null);

            this.resolverFactory = resolverFactory;
        }

        public Option<Res, Exception> Execute(Req req)
        {
            var resolver = this.resolverFactory(req.VersionFile);

            this.log.Info("Attempting to resolve version from {0}", req.VersionFile);
            var res = from v in resolver.Resolve()
                      select CreateResponse(req, v);

            res.MatchSome(x =>
            {
                this.log.Info("Found version {0} from {1}", x.VersionName, x.VersionFile);
            });

            return res;
        }

        private static Res CreateResponse(Req req, string version)
        {
            return new Res
            {
                VersionFile = req.VersionFile,
                VersionName = version,
            };
        }
    }
}