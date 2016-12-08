namespace Yuki
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Newtonsoft.Json;

    public static class Utils
    {
        public static KeyValuePair<TKey,TValue> CreateKeyValuePair<TKey,TValue>(TKey key, TValue value)
        {
            return new KeyValuePair<TKey, TValue>(key, value);
        }

        public static ScriptType ParseScriptType(string value, ScriptType defaultScriptType)
        {
            ScriptType st;
            if (TryParseScriptType(value, out st))
            {
                return st;
            }

            return defaultScriptType;
        }

        public static bool TryParseScriptType(string value, out ScriptType result)
        {
            if (Enum.TryParse<ScriptType>(value, out result))
            {
                return true;
            }

            return false;
        }

        public static Tuple<string, Config> ReadConfiguration(string path)
        {
            var json = File.ReadAllText(path);
            var obj = JsonConvert.DeserializeObject<Config>(json);
            return Tuple.Create(json, obj);
        }

        public static string ReadEmbeddedString<T>(string resourceName)
        {
            var asm = typeof(T).Assembly;
            resourceName = $"Yuki.{resourceName}";
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
