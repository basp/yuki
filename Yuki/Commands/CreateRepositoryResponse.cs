namespace Yuki.Commands
{
    using System.Diagnostics.Contracts;

    public class CreateRepositoryResponse
    {
        public CreateRepositoryResponse(
            string database,
            string schema)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(database));
            Contract.Requires(!string.IsNullOrWhiteSpace(schema));

            this.Database = database;
            this.Schema = schema;
        }

        public string Database
        {
            get;
            private set;
        }

        public string Schema
        {
            get;
            private set;
        }
    }
}
