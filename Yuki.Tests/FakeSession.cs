namespace Yuki.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Optional;

    using static Optional.Option;

    public class FakeSession : ISession
    {
        public int CommandTimeout
        {
            get;
            set;
        }

        public void BeginTransaction()
        {
        }

        public void CommitTransaction()
        {
        }

        public void Dispose()
        {
        }

        public int ExecuteNonQuery(
            string cmdText,
            IDictionary<string, object> args,
            CommandType commandType)
        {
            throw new NotImplementedException();
        }

        public T ExecuteScalar<T>(
            string cmdText,
            IDictionary<string, object> args,
            CommandType commandType)
        {
            throw new NotImplementedException();
        }

        public void Open()
        {
        }

        public void RollbackTransaction()
        {
        }

        public Option<int, Exception> TryExecuteNonQuery(
            string cmdText,
            IDictionary<string, object> args,
            CommandType commandType)
        {
            throw new NotImplementedException();
        }

        public Option<T, Exception> TryExecuteScalar<T>(
            string cmdText,
            IDictionary<string, object> args,
            CommandType commandType)
        {
            throw new NotImplementedException();
        }
    }
}
