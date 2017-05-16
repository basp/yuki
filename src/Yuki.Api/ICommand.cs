namespace Yuki.Api
{
    using Optional;

    public interface ICommand<TReq, TRes, TException>
    {
        Option<TRes, TException> Execute(TReq req);
    }
}
