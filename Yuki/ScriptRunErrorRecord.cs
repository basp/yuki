namespace Yuki
{
    public interface IScriptRunErrorRecord
    {
        string ScriptName { get; set; }

        string Sql { get; set; }

        string SqlErrorPart { get; set; }

        string ErrorMessage { get; set; }

        string RepositoryVersion { get; set; }

        string RepositoryPath { get; set; }

        string EnteredBy { get; set; }
    }
}
