namespace Yuki
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class Folder
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string TypeString { get; set; }

        [JsonIgnore]
        public ScriptType Type
        {
            get
            {
                ScriptType v;
                if (Utils.TryParseScriptType(this.TypeString, out v))
                {
                    return v;
                }

                return ScriptType.Unknown;
            }
        }

        [JsonProperty(PropertyName = "folders")]
        public IEnumerable<string> Folders { get; private set; }
    }
}
