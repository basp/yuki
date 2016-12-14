namespace Yuki
{
    using System.ComponentModel;
    using Newtonsoft.Json;

    public class DefaultConfig : IProjectConfig
    {
        [JsonProperty(PropertyName = "databaseFolder", DefaultValueHandling = DefaultValueHandling.Populate)]
        [DefaultValue("databases")]
        public string DatabaseFolder
        {
            get;
            set;
        }
    }
}
