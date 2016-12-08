namespace Yuki
{
    public interface ITransaction
    {
        void Commit();

        void Rollback();
    }
}
