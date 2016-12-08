namespace Yuki.Cmd
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    public interface ISession : IDisposable
    {
        int CommandTimeout { get; set; }

        void BeginTransaction();

        void CommitTransaction();

        void RollbackTransaction();

        object ExecuteScalar(
            string cmdText,
            IDictionary<string, object> args,
            CommandType commandType);

        int ExecuteNonQuery(
            string cmdText,
            IDictionary<string, object> args,
            CommandType commandType);
    }
}
