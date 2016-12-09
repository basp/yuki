namespace Yuki
{
    using System.ComponentModel;
    using Newtonsoft.Json;

    public class Config
    {
        [DefaultValue(Default.SystemDatabase)]
        [JsonProperty(
            PropertyName = "systemDatabase", 
            DefaultValueHandling = DefaultValueHandling.Populate)]
        public string SystemDatabase { get; set; }

        [DefaultValue(Default.SystemSchema)]
        [JsonProperty(
            PropertyName = "systemSchema", 
            DefaultValueHandling = DefaultValueHandling.Populate)]
        public string SystemSchema { get; set; }
     
        [DefaultValue(Default.DatabaseFolder)]
        [JsonProperty(
            PropertyName = "databaseFolder",
            DefaultValueHandling = DefaultValueHandling.Populate)]
        public string DatabaseFolder { get; set; }

        [DefaultValue("version.txt")]
        [JsonProperty(
            PropertyName = "versionFile",
            DefaultValueHandling = DefaultValueHandling.Populate)]
        public string VersionFile { get; set; }

        [DefaultValue(120)]
        [JsonProperty(
            PropertyName = "restoreTimeout",
            DefaultValueHandling = DefaultValueHandling.Populate)]
        public int RestoreTimeout { get; set; }

        [DefaultValue(15)]
        [JsonProperty(
            PropertyName = "commandTimeout",
            DefaultValueHandling = DefaultValueHandling.Populate)]
        public int CommandTimeout { get; set; }

        [JsonProperty(PropertyName = "folders")]
        public Folder[] Folders { get; set; }
    }
}
