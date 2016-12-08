namespace Yuki.Cmd
{
    using System.Collections.Generic;

    public class Folder
    {
        public string Name { get; set; }

        public ScriptType Type { get; set; }

        public IEnumerable<string> Folders { get; private set; }
    }
}
