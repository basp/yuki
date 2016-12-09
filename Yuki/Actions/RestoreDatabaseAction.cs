namespace Yuki.Actions
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Maybe;
 
    public enum RestoreDatabaseResult
    {
        Restored,
        Error,
        None
    }

    public class RestoreDatabaseAction : IAction<RestoreDatabaseArgs, RestoreDatabaseResult>
    {
        public IMaybeError<RestoreDatabaseResult> Execute(RestoreDatabaseArgs args)
        {
            var template = Utils.ReadEmbeddedString<Program>("RestoreDatabase.sql");
            var cmdText = string.Format(
                template, 
                args.DatabaseName, 
                args.BackupFile);

            try
            {
                args.Server.ExecuteNonQuery(
                    cmdText,
                    new Dictionary<string, object>(),
                    CommandType.Text);
            }
            catch(Exception ex)
            {
                return MaybeError.Create(
                    RestoreDatabaseResult.Error,
                    ex);
            }

            return MaybeError.Create(RestoreDatabaseResult.None);
        }
    }
}
