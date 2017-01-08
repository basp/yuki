namespace Yuki
{
    public interface IDatabaseFactory
    {
        IDatabase Create(string name);
    }
}
