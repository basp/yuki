using System.Configuration;

namespace Yuki.Cmd
{
    public static class Config
    {
        public static string Server
        {
            get => ConfigurationManager.AppSettings.Get("server");
        }

        public static string Username
        {
            get => ConfigurationManager.AppSettings.Get("username");
        }

        public static string Password
        {
            get => ConfigurationManager.AppSettings.Get("password");
        }
    }
}
