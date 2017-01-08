namespace Yuki.Commands
{
    public class HasScriptRunResponse
    {
        public string RepositoryDatabase { get; set; }

        public string RepositorySchema { get; set; }

        public string ScriptName { get; set; }

        public bool HasRunAlready { get; set; }
    }
}
