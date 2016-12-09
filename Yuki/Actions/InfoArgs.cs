namespace Yuki.Actions
{
    using PowerArgs;

    public class InfoArgs
    {
        [ArgDescription("The folder to inspect")]
        [ArgDefaultValue(".")]    
        [ArgShortcut("f")]    
        public string Folder { get; set; }
    }
}
