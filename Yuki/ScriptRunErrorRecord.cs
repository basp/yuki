namespace Yuki
{
    public class ScriptRunErrorRecord
    {
        public string RepositoryPath { get; set; }

        public string ScriptName { get; set; }

        public string VersionName { get; set; }

        public string Sql { get; set; }

        public string SqlErrorPart { get; set; }

        public string ErrorMessage { get; set; }

        public string EnteredBy { get; set; }
    }
}
