namespace Yuki.Commands
{
    public class InsertScriptRunErrorRequest
    {
        public string RepositoryDatabase { get; set; }

        public string RepositorySchema { get; set; }

        public string RepositoryPath { get; set; }

        public string ScriptName { get; set; }

        public string VersionName { get; set; }

        public string Sql { get; set; }

        public string SqlErrorPart { get; set; }

        public string ErrorMessage { get; set; }
    }
}
