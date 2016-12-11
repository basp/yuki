namespace Yuki.Commands
{
    public interface IRepositoryResponse : IDatabaseResponse
    {
        string Schema { get; set; }
    }
}
