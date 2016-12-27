namespace Yuki
{
    using System;
    using System.Security.Principal;
    using Optional;

    using static Optional.Option;

    public class WindowsIdentityProvider : IIdentityProvider
    {
        public Option<string, Exception> GetCurrent()
        {
            try
            {
                var current = WindowsIdentity.GetCurrent();
                return Some<string, Exception>(current.Name);
            }
            catch (Exception ex)
            {
                return None<string, Exception>(ex);
            }
        }
    }
}