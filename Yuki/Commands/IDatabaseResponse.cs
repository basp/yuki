namespace Yuki.Commands
{
    public interface IDatabaseResponse
    {
        string Server { get; }

        string Database { get; }
    }
}
