namespace Yuki
{
    using System;
    using System.IO;

    public class Context
    {
        public const string DefaultConfigFile = "yuki.json";

        public Config Config { get; set; }

        public string ConfigString { get; set; }

        public string ProjectFile { get; set; }

        public string ProjectDirectory { get; set; }

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
                ProjectFile = cf,
                ProjectDirectory = cwd
            };
        }

        private static Tuple<string, Config> ReadConfiguration(string path)
        {
            return Utils.ReadConfiguration(path);
        }
    }
}
