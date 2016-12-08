namespace Yuki
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;

    public class SqlSession : ISession
    {
        private readonly IDbConnection connection;

        private IDbTransaction transaction;

        private SqlSession(IDbConnection connection)
        {
            this.connection = connection;
        }

        public int CommandTimeout { get; set; }

        public static ISession Open(string connectionString)
        {
            var conn = new SqlConnection(connectionString);
            conn.Open();
            return new SqlSession(conn);
        }

        public void BeginTransaction()
        {
            this.transaction = this.connection.BeginTransaction();
        }

        public void RollbackTransaction()
        {
            if (this.transaction == null)
            {
                return;
            }

            this.transaction.Rollback();
            this.transaction = null;
        }

        public void CommitTransaction()
        {
            if (this.transaction == null)
            {
                return;
            }

            this.transaction.Commit();
            this.transaction = null;
        }

        public object ExecuteScalar(
            string commandText,
            IDictionary<string, object> args,
            CommandType commandType)
        {
            using (var cmd = this.CreateCommand(commandText, args, commandType))
            {
                return cmd.ExecuteScalar();
            }
        }

        public int ExecuteNonQuery(
            string commandText,
            IDictionary<string, object> args,
            CommandType commandType)
        {
            using (var cmd = this.CreateCommand(commandText, args, commandType))
            {
                return cmd.ExecuteNonQuery();
            }
        }

        public void Dispose()
        {
            this.connection.Close();
            this.connection.Dispose();

            if (this.transaction != null)
            {
                this.transaction.Dispose();
            }
        }

        private IDbCommand CreateCommand(
            string commandText,
            IDictionary<string, object> args,
            CommandType commandType)
        {
            var cmd = this.connection.CreateCommand();
            cmd.CommandText = commandText;
            cmd.CommandType = commandType;
            cmd.CommandTimeout = this.CommandTimeout;
            cmd.Transaction = this.transaction;
            foreach (var a in args)
            {
                var p = cmd.CreateParameter();
                p.ParameterName = a.Key;
                p.Value = a.Value;
                cmd.Parameters.Add(p);
            }

            return cmd;
        }
    }
}
