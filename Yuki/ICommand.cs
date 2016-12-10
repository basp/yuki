namespace Yuki
{
    using Optional;

    public interface ICommand<TRequest, TResponse, TException>
    {
        Option<TResponse, TException> Execute(TRequest request);
    }
}
