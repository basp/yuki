namespace Yuki.Commands
{
    public class InsertScriptRunResponse
    {
        public int VersionId { get; set; }

        public string ScriptName { get; set; }

        public bool IsOneTimeScript { get; set; }

        public string RepositoryDatabase { get; set; }

        public string RepositorySchema { get; set; }

        public int ScriptRunId { get; set; }
    }
}
