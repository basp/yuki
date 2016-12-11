namespace Yuki
{
    public interface IVersionRecord
    {
        string RepositoryPath { get; set; }

        string VersionName { get; set; }

        string EnteredBy { get; set; }
    }
}
