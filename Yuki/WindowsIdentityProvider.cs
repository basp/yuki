namespace Yuki
{
    using System.Security.Principal;
    using Optional;

    public class WindowsIdentityProvider : IIdentityProvider
    {
        public Option<string> GetCurrent()
        {
            return WindowsIdentity.GetCurrent()
                .SomeNotNull()
                .Map(x => x.Name);
        }
    }
}
