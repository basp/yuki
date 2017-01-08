namespace Yuki
{
    public interface IVersionResolverFactory
    {
        IVersionResolver Create(string versionFile);
    }
}
