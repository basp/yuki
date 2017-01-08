namespace Yuki
{
    using System;
    using Optional;

    public interface IRepository
    {
        Option<bool, Exception> Initialize();

        Option<int, Exception> InsertScriptRun(
            ScriptRunRecord record);

        Option<int, Exception> InsetScriptRunError(
            ScriptRunErrorRecord record);

        Option<int, Exception> InsertVersion(
            VersionRecord record);

        Option<bool, Exception> HasScriptRun(
            string scriptName);

        Option<string, Exception> GetCurrentHash(
            string scriptName);

        Option<string, Exception> GetVersion(
            string repositoryPath);
    }
}
