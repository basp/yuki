namespace Yuki.Commands
{
    using System;
    using System.IO;
    using PowerArgs;

    public class SetupDatabaseRequest
    {
        [ArgRequired]
        [ArgPosition(1)]
        public string Server { get; set; }

        [ArgRequired]
        [ArgPosition(2)]
        public string Folder { get; set; }

        [ArgIgnore]
        public string Database
        {
            get
            {
                return Path.GetFileName(this.Folder);
            }
        }
    }
}
