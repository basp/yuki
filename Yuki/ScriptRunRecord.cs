namespace Yuki
{
    public class ScriptRunRecord<TIdentity>
    {
        public string ScriptName { get; set; }

        public string Sql { get; set; }

        public string Hash { get; set; }

        public bool IsOneTimeScript { get; set; }

        public TIdentity VersionId { get; set; }
    }
}
