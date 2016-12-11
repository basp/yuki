namespace Yuki
{
    using System;
    using Optional;

    public interface IRepository<TIdentity, TException>
    {
        Option<TIdentity, TException> InsertVersion(
            IVersionRecord record);

        Option<TIdentity, TException> InsertScriptRun(
            IScriptRunRecord<TIdentity> record);

        Option<TIdentity, TException> InsertScriptRunError(
            IScriptRunErrorRecord record);

        Option<bool, TException> HasScriptRunAlready(
            string scriptName);

        Option<string, TException> GetVersion(
            string repositoryPath);

        Option<string, TException> GetHash(
            string scriptName);
    }
}
