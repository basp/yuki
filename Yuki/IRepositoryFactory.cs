namespace Yuki
{
    public interface IRepositoryFactory
    {
        IRepository Create(string repositoryDatabase, string repositorySchema);
    }
}
