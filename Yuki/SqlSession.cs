﻿namespace Yuki
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;

    public class SqlSession : ISession
    {
        private IDbTransaction transaction;
        private bool disposed = false;

        public SqlSession(IDbConnection connection)
        {
            Contract.Requires(connection != null);
            Contract.Requires(connection.State == ConnectionState.Closed);

            this.Connection = connection;
        }

        public IDbConnection Connection
        {
            get;
            private set;
        }

        public int CommandTimeout
        {
            get;
            set;
        }

        public void Open()
        {
            this.Connection.Open();
        }

        public void BeginTransaction()
        {
            this.transaction = this.Connection.BeginTransaction();
        }

        public void CommitTransaction()
        {
            this.transaction.Commit();
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

        public int ExecuteNonQuery(
            string cmdText,
            IDictionary<string, object> args,
            CommandType commandType)
        {
            using (var cmd = this.CreateCommand(cmdText, args, commandType))
            {
                return cmd.ExecuteNonQuery();
            }
        }

        public object ExecuteScalar(
            string cmdText,
            IDictionary<string, object> args,
            CommandType commandType)
        {
            using (var cmd = this.CreateCommand(cmdText, args, commandType))
            {
                return cmd.ExecuteScalar();
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        [SuppressMessage(
            "Microsoft.Security",
            "CA2100:Review SQL queries for security vulnerabilities",
            Justification = "This is handled by commands individually")]
        private IDbCommand CreateCommand(
            string cmdText,
            IDictionary<string, object> args,
            CommandType commandType)
        {
            var cmd = this.Connection.CreateCommand();

            cmd.CommandText = cmdText;
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

        private void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            if (disposing)
            {
                this.Connection.Close();
                this.Connection.Dispose();

                if (this.transaction != null)
                {
                    this.transaction.Dispose();
                }
            }

            this.disposed = true;
        }
    }
}
