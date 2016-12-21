namespace Yuki.Commands
{
    using System;
    using System.Diagnostics.Contracts;
    using System.IO;
    using Optional;

    using static Optional.Option;

    using Req = InsertScriptRunRequest;
    using Res = InsertScriptRunResponse;

    public class InsertScriptRunCommand : ICommand<Req, Res, Exception>
    {
        private readonly ISession session;
        private readonly IIdentityProvider identity;
        private readonly ICommand<ReadFileRequest, ReadFileResponse, Exception> readFileCommand;

        public InsertScriptRunCommand(
            ISession session,
            IIdentityProvider identity,
            ICommand<ReadFileRequest, ReadFileResponse, Exception> readFileCommand)
        {
            Contract.Requires(session != null);
            Contract.Requires(identity != null);
            Contract.Requires(readFileCommand != null);

            this.session = session;
            this.identity = identity;
            this.readFileCommand = readFileCommand;
        }

        public Option<Res, Exception> Execute(Req req)
        {
            return this.identity.GetCurrent()
                .WithException(() => new Exception())
                .FlatMap(x =>
                {
                    req.EnteredBy = x;
                    return GetFullPath(req.ScriptName);
                })
                .FlatMap(x =>
                {
                    var readFileReq = new ReadFileRequest() { Path = x };
                    return this.readFileCommand.Execute(readFileReq);
                })
                .FlatMap(x =>
                {
                    req.Hash = x.Hash;
                    req.Sql = x.Contents;
                    var repo = new SqlRepository(this.session, req);
                    return repo.InsertScriptRun(req);
                })
                .Map(x => CreateResult(x, req));
        }

        private static Option<string, Exception> GetFullPath(string file)
        {
            try
            {
                var path = Path.GetFullPath(file);
                return Some<string, Exception>(path);
            }
            catch (Exception ex)
            {
                return None<string, Exception>(ex);
            }
        }

        private static Res CreateResult(int scriptRunId, Req req)
        {
            return new Res()
            {
                Server = req.Server,
                Database = req.RepositoryDatabase,
                Schema = req.RepositorySchema,
                ScriptRunId = scriptRunId,
                EnteredBy = req.EnteredBy,
                Hash = req.Hash,
                Sql = req.Sql,
                IsOneTimeScript = req.IsOneTimeScript,
                ScriptName = req.ScriptName,
                VersionId = req.VersionId,
            };
        }
    }
}
