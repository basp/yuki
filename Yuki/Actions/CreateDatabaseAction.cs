namespace Yuki.Actions
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Maybe;
    using NLog;
 
    public enum CreateDatabaseResult
    {
        Created,
        Exists,
        Error,
        None
    }

    public class CreateDatabaseAction : IAction<CreateDatabaseArgs, CreateDatabaseResult>
    {
        private readonly ILogger log = LogManager.GetCurrentClassLogger();
        private readonly ISession session;

        public CreateDatabaseAction(ISession session)
        {
            this.session = session;
        }

        public IMaybeError<CreateDatabaseResult> Execute(CreateDatabaseArgs args)
        {
            var result = CreateDatabaseResult.None;
            try
            {
                var resourceName = $"CreateDatabase.sql";
                var template = Utils.ReadEmbeddedString<Program>(resourceName);
                var cmdText = string.Format(template, args.Name);
                var statements = StatementSplitter.Split(cmdText);
                foreach (var s in statements)
                {
                    bool val;
                    if (this.TryExecuteScalar(s, out val))
                    {
                        result = val
                            ? CreateDatabaseResult.Created
                            : CreateDatabaseResult.Exists;
                    }
                }
            }
            catch (Exception ex)
            {
                return MaybeError.Create(CreateDatabaseResult.Error, ex);
            }

            return MaybeError.Create(result);
        }

        private bool TryExecuteScalar<T>(string cmdText, out T scalar) where T : struct
        {
            scalar = default(T);

            var result = this.session.ExecuteScalar(
                cmdText,
                new Dictionary<string, object>(),
                CommandType.Text);

            if (result == null || result == DBNull.Value)
            {
                return false;
            }

            try
            {
                scalar = (T)result;
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
