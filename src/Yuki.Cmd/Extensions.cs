using IdentityModel.Client;

namespace Yuki.Cmd
{
    public static class Extensions
    {
        public static TokenResponse GetClientToken<T>(
            this IAction<T> action,
            TokenClient tokenClient) =>
                tokenClient
                    .RequestResourceOwnerPasswordAsync(
                        Config.Username,
                        Config.Password,
                        "api")
                    .Result;
    }
}
