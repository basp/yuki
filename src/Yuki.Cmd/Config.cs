using System.Configuration;

namespace Yuki.Cmd
{
    public static class Config
    {
        public static string ApiEndPoint
        {
            get => ConfigurationManager.AppSettings.Get("apiEndPoint");
        }

        public static string TokenEndPoint
        {
            get => ConfigurationManager.AppSettings.Get("tokenEndPoint");
        }

        public static string Username
        {
            get => ConfigurationManager.AppSettings.Get("username");
        }

        public static string Password
        {
            get => ConfigurationManager.AppSettings.Get("password");
        }

        public static string ClientId
        {
            get => ConfigurationManager.AppSettings.Get("clientId");
        }

        public static string ClientSecret
        {
            get => ConfigurationManager.AppSettings.Get("clientSecret");
        }
    }
}
