namespace Yuki.Commands
{
    public class GetCurrentHashResponse
    {
        public string RepositoryDatabase { get; set; }

        public string RepositorySchema { get; set; }

        public string ScriptName { get; set; }

        public string Hash { get; set; }
    }
}
