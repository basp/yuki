namespace Yuki
{
    public interface IRepository
    {
        void Initialize();

        int InsertVersion(
            string repositoryPath,
            string repositoryVersion);

        int InsertScriptRun(
            string scriptName,
            string sql,
            bool isOneTimeScript,
            int versionId);

        int InsertScriptRunError(
            string scriptName,
            string sql,
            string sqlErrorPart,
            string errorMessage,
            string repositoryVersion,
            string repositoryPath);

        bool HasScriptRunAlready(string scriptName);

        string GetVersion(string repositoryPath);

        string GetHash(string scriptName);
    }
}
