namespace Yuki
{
    public class ScriptRunErrorRecord
    {
        public string ScriptName { get; set; }

        public string Sql { get; set; }

        public string SqlErrorPart { get; set; }

        public string ErrorMessage { get; set; }

        public string RepositoryVersion { get; set; }

        public string RepositoryPath { get; set; }
    }
}
