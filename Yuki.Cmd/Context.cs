
namespace Yuki.Cmd
{
    using System;
    using System.IO;

    public class Context
    {
        public const string DefaultConfigFile = "yuki.json";

        public dynamic Config { get; set; }
        public string ConfigString { get; set; }
        public string ConfigFile { get; set; }
        public string CurrentDirectory { get; set; }
        public static Context GetCurrent()
        {
            var cwd = Directory.GetCurrentDirectory();
            var cf = Path.Combine(cwd, DefaultConfigFile);
            var t = ReadConfiguration(cf);
            var json = t.Item1;
            var cfg = t.Item2;

            return new Context()
            {
                Config = cfg,
                ConfigString = json,
                ConfigFile = cf,
                CurrentDirectory = cwd
            };
        }

        static Tuple<string, dynamic> ReadConfiguration(string path)
        {
            return Utils.ReadConfiguration(path);
        }
    }
}
