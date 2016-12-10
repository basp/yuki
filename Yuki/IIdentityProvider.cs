namespace Yuki
{
    using Optional;

    public interface IIdentityProvider
    {
        Option<string> GetCurrent();
    }
}
