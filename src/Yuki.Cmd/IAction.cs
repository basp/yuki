namespace Yuki.Cmd
{
    using System.Threading.Tasks;

    public interface IAction<TArgs>
    {
        Task Execute(TArgs args);
    }
}
