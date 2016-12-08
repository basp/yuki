namespace Yuki
{
    public interface IAction<TArgs>
    {
        void Execute(TArgs args);
    }
}
