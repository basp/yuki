namespace Yuki
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Optional;

    public interface ISession : IDisposable
    {
        int CommandTimeout { get; set; }

        void Open();

        void BeginTransaction();

        void CommitTransaction();

        void RollbackTransaction();

        T ExecuteScalar<T>(
            string cmdText,
            IDictionary<string, object> args,
            CommandType commandType);

        int ExecuteNonQuery(
            string cmdText,
            IDictionary<string, object> args,
            CommandType commandType);

        Option<T, Exception> TryExecuteScalar<T>(
            string cmdText,
            IDictionary<string, object> args,
            CommandType commandType);

        Option<int, Exception> TryExecuteNonQuery(
            string cmdText,
            IDictionary<string, object> args,
            CommandType commandType);
    }
}
