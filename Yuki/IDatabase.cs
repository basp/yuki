namespace Yuki
{
    public interface IDatabase
    {
        bool CreateDatabaseIfNotExists(string databaseName);

        void RestoreDatabase(string databaseName, string restorePath);
    }
}
