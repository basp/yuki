namespace Yuki
{
    using System;
    using System.IO;
    using Newtonsoft.Json;
 
    public class Context
    {
        [JsonProperty(PropertyName = "config")]
        public Config Config { get; set; }

        [JsonProperty(PropertyName = "configString")]
        public string ConfigString { get; set; }

        [JsonProperty(PropertyName = "projectFile")]
        public string ProjectFile { get; set; }

        [JsonProperty(PropertyName = "projectDirectory")]
        public string ProjectDirectory { get; set; }

        public static Context GetCurrent()
        {
            var cwd = Directory.GetCurrentDirectory();
            var cf = Path.Combine(cwd, Default.ConfigFile);
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
