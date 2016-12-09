namespace Yuki
{
    using Maybe;

    public interface IAction<TArgs, TResult>
    {
        IMaybeError<TResult> Execute(TArgs args);
    }
}
