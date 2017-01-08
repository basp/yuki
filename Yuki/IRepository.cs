namespace Yuki
{
    using System;
    using Optional;

    public interface IRepository
    {
        Option<bool, Exception> Initialize();

        Option<string, Exception> GetCurrentHash(string scriptName);

        Option<bool, Exception> HasScriptRun(string scriptName);

        Option<string, Exception> GetVersion(string repositoryPath);
    }
}
