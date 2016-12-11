namespace Yuki.Commands
{
    using System;
    using Newtonsoft.Json;

    public class InsertVersionResponse : IRepositoryResponse
    {
        public string Server { get; set; }

        public string Database { get; set; }

        public string Schema { get; set; }

        public int VersionId { get; set; }

        public string VersionName { get; set; }

        public string RepositoryPath { get; set; }

        public string EnteredBy { get; set; }
    }
}
