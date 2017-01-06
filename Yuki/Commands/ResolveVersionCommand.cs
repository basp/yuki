namespace Yuki.Commands
{
    using System;
    using System.Diagnostics.Contracts;
    using Optional;
    using Optional.Linq;
    using Serilog;

    using Req = ResolveVersionRequest;
    using Res = ResolveVersionResponse;

    public class ResolveVersionCommand
        : IResolveVersionCommand
    {
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

            Log.Information("Attempting to resolve version from {VersionSource}", req.VersionFile);
            var res = from v in resolver.Resolve()
                      select CreateResponse(req, v);

            res.MatchSome(x =>
            {
                Log.Information(
                    "Resolved version {NextVersion} from {VersionSource}",
                    x.VersionName,
                    x.VersionFile);
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