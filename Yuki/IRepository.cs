namespace Yuki
{
    using System;
    using Optional;

    public interface IRepository<TIdentity, TException>
    {
        Option<TIdentity, TException> InsertVersion(
            VersionRecord record);

        Option<TIdentity, TException> InsertScriptRun(
            ScriptRunRecord<TIdentity> record);

        Option<TIdentity, TException> InsertScriptRunError(
            ScriptRunErrorRecord record);

        Option<bool, TException> HasScriptRunAlready(
            string scriptName);

        Option<string, TException> GetVersion(
            string repositoryPath);

        Option<string, TException> GetHash(
            string scriptName);
    }
}
