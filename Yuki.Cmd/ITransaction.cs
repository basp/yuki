namespace Yuki.Cmd
{
    public interface ITransaction
    {
        void Commit();

        void Rollback();
    }
}
