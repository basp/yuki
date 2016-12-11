namespace Yuki.Commands
{
    public class GetVersionResponse : IRepositoryResponse
    {
        public string Server { get; set; }

        public string Database { get; set; }

        public string Schema { get; set; }

        public string RepositoryPath { get; set; }

        public string VersionName { get; set; }
    }
}
