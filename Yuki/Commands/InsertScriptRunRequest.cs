namespace Yuki.Commands
{
    public class InsertScriptRunRequest
    {
        public int VersionId { get; set; }

        public string ScriptName { get; set; }

        public string Sql { get; set; }

        public string Hash { get; set; }

        public bool IsOneTimeScript { get; set; }

        public string RepositoryDatabase { get; set; }

        public string RepositorySchema { get; set; }
    }
}
