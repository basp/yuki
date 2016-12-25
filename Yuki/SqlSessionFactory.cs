namespace Yuki
{
    using System;
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.Diagnostics.Contracts;

    public class SqlSessionFactory : ISessionFactory
    {
        private readonly DbConnectionStringBuilder connectionStringBuilder;

        public SqlSessionFactory(DbConnectionStringBuilder connectionStringBuilder)
        {
            Contract.Requires(connectionStringBuilder != null);
            this.connectionStringBuilder = connectionStringBuilder;
        }

        public ISession Create()
        {
            var connectionString = this.connectionStringBuilder.ConnectionString;
            var connection = new SqlConnection(connectionString);
            return new SqlSession(connection);
        }
    }
}
