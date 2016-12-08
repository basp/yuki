namespace Yuki.Actions
{
    using PowerArgs;

    public class RestoreArgs
    {
        [ArgDescription("Folder containing the database specific scripts and/or backups to restore")]
        [ArgDefaultValue(Default.DatabaseFolder)]
        [ArgShortcut("f")]
        [ArgPosition(1)]
        public string Folder { get; set; }
    }
}
