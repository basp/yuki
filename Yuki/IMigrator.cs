namespace Yuki
{
    using System;
    using Commands;
    using Optional;

    public interface IMigrator
    {
        Option<bool, Exception> RunMigrationScripts(
            string scriptFolder,
            string newVersion,
            int versionId,
            bool isOneTimeFolder = false,
            bool isEveryTimeFolder = false);

        Option<InsertVersionResponse, Exception> InsertNextVersion(
            string currentVersion,
            string nextVersion);

        Option<GetVersionResponse, Exception> GetCurrentVersion();

        Option<ResolveVersionResponse, Exception> ResolveNextVersion();
    }
}