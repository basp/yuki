namespace Yuki.Commands
{
    public class HasScriptRunRequest
    {
        public string RepositoryDatabase { get; set; }

        public string RepositorySchema { get; set; }

        public string ScriptName { get; set; }
    }
}
