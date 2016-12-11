namespace Yuki.Commands
{
    using Newtonsoft.Json;

    public class InsertVersionResponse
    {
        public int VersionId { get; set; }

        public string VersionName { get; set; }

        public string RepositoryPath { get; set; }

        public string EnteredBy { get; set; }
    }
}
