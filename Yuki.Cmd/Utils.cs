namespace Yuki.Cmd
{
    using System;
    using System.IO;
    using Newtonsoft.Json;

    public static class Utils
    {
        public static Tuple<string,dynamic> ReadConfiguration(string path)
        {
            var json = File.ReadAllText(path);
            var obj = JsonConvert.DeserializeObject(json);
            return Tuple.Create(json, obj);
        }

        public static string ReadEmbeddedString<T>(string resourceName)
        {
            var asm = typeof(T).Assembly;
            resourceName = $"Yuki.Cmd.{resourceName}";
            using (var stream = asm.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    var msg = $"Could not find embedded resource with name '{resourceName}'";
                    throw new ArgumentException(msg, nameof(resourceName));
                }

                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
