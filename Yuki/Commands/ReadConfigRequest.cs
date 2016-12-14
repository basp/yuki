namespace Yuki.Commands
{
    using PowerArgs;

    public class ReadConfigRequest
    {
        [ArgPosition(1)]
        [ArgDefaultValue(@".\yuki.json")]
        public string ConfigFile { get; set; }
    }
}
