namespace Yuki.Actions
{
    public class RestoreDatabaseArgs
    {
        public string BackupFile { get; set; }

        public string DatabaseName { get; set; }

        public ISession Server { get; set; }
    }
}
