namespace Yuki
{
    public interface IAction<TArgs>
    {
        IMaybeError Execute(TArgs args);
    }
}
