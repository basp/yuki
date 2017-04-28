namespace Yuki.IdentityServer
{
    using System;
    using Microsoft.Owin.Hosting;
    using Serilog;

    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.LiterateConsole()
                .CreateLogger();

            using (WebApp.Start<Startup>("http://localhost:5000"))
            {
                Console.ReadLine();
            }
        }
    }
}