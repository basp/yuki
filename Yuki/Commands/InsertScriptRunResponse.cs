namespace Yuki.Commands
{
    public class InsertScriptRunResponse : IRepositoryResponse
    {
        public string Database { get; set; }

        public string Schema { get; set; }

        public string Server { get; set; }

        public int ScriptRunId { get; set; }

        public string EnteredBy { get; set; }

        public int VersionId { get; set; }

        public string ScriptName { get; set; }

        public string Hash { get; set; }

        public bool IsOneTimeScript { get; set; }
    }
}
