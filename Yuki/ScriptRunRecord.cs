namespace Yuki
{
    public interface IScriptRunRecord<TIdentity>
    {
        string ScriptName { get; set; }

        string Sql { get; set; }

        string Hash { get; set; }

        bool IsOneTimeScript { get; set; }

        TIdentity VersionId { get; set; }

        string EnteredBy { get; set; }
    }
}
