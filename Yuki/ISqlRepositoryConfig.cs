namespace Yuki
{
    public interface ISqlRepositoryConfig
    {
        string RepositoryDatabase
        {
            get;
        }

        string RepositorySchema
        {
            get;
        }
    }
}
