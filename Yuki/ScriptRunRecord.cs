namespace Yuki
{
    public class ScriptRunRecord
    {
        public int VersionId { get; set; }

        public string ScriptName { get; set; }

        public string Sql { get; set; }

        public string Hash { get; set; }

        public bool IsOneTimeScript { get; set; }

        public string EnteredBy { get; set; }
    }
}
