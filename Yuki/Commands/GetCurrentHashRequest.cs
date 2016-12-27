namespace Yuki.Commands
{
    public class GetCurrentHashRequest
    {
        public string RepositoryDatabase { get; set; }

        public string RepositorySchema { get; set; }

        public string ScriptName { get; set; }
    }
}
