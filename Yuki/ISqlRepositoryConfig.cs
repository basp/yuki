namespace Yuki
{
    public interface ISqlRepositoryConfig
    {
        string Database
        {
            get;
        }

        string Schema
        {
            get;
        }
    }
}
