namespace Yuki
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Security.Principal;
    using NLog;

    public class SqlRepository : IRepository
    {
        private readonly ILogger log = LogManager.GetCurrentClassLogger();
     
        private readonly ISession session;
        private readonly string systemDatabase;
        private readonly string systemSchema;

        public SqlRepository(
            ISession session,
            string systemDatabase = Default.SystemDatabase,
            string systemSchema = Default.SystemSchema)
        {
            this.session = session;
            this.systemDatabase = systemDatabase;
            this.systemSchema = systemSchema;
        }

        public void Initialize(string systemDatabase, string systemSchema)
        {
            this.log.Info($"Creating repository database [{this.systemDatabase}] if it doesn't exist");

            var resourceName = "CreateSystemDatabase.sql";
            var template = Utils.ReadEmbeddedString<Program>(resourceName);
            var sql = string.Format(
                template,
                systemDatabase,
                systemSchema);

            try
            {
                var parts = StatementSplitter.Split(sql);
                foreach (var p in parts)
                {
                    this.session.ExecuteNonQuery(
                        p, 
                        new Dictionary<string, object>(), 
                        CommandType.Text);
                }
            }
            catch (Exception ex)
            {
                this.log.Error(ex, ex.Message);
                throw;
            }
        }

        public int InsertVersion(string repositoryPath, string repositoryVersion)
        {
            var sp = this.FullyQualifiedObjectName("InsertVersion");
            var args = new Dictionary<string, object>
            {
                { "VersionName", repositoryVersion },
                { "RepositoryPath", repositoryPath },
                { "EnteredBy", GetIdentity() }
            };

            return (int)this.session.ExecuteScalar(sp, args, CommandType.StoredProcedure);
        }

        public int InsertScriptRun(
            string scriptName, 
            string sql, 
            string hash, 
            bool isOneTimeScript, 
            int versionId)
        {
            var sp = this.FullyQualifiedObjectName("InsertScriptRun");
            var args = new Dictionary<string, object>
            {
                { "VersionId", versionId },
                { "ScriptName", scriptName },
                { "TextOfScript", sql },
                { "TextHash", hash },
                { "OneTimeScript", isOneTimeScript },
                { "EnteredBy", GetIdentity() }
            };

            return (int)this.session.ExecuteScalar(sp, args, CommandType.StoredProcedure);
        }

        public int InsertScriptRunError(
            string scriptName, 
            string sql, 
            string sqlErrorPart, 
            string errorMessage, 
            string repositoryVersion, 
            string repositoryPath)
        {
            var sp = this.FullyQualifiedObjectName("InsertScriptRunError");
            var args = new Dictionary<string, object>
            {
                { "RepositoryPath", repositoryPath },
                { "ScriptName", scriptName },
                { "VersionName", repositoryVersion },
                { "TextOfScript", sql },
                { "ErroneousPart", sqlErrorPart },
                { "ErrorMessage", errorMessage },
                { "EnteredBy", GetIdentity() }
            };

            return (int)this.session.ExecuteScalar(sp, args, CommandType.StoredProcedure);
        }

        public bool HasScriptRunAlready(string scriptName)
        {
            var sp = this.FullyQualifiedObjectName("HasScriptRunAlready");
            var args = new Dictionary<string, object>
            {
                { "ScriptName", scriptName }
            };

            var count = (int)this.session.ExecuteScalar(sp, args, CommandType.StoredProcedure);
            return count > 0;
        }

        public string GetVersion(string repositoryPath)
        {
            var sp = this.FullyQualifiedObjectName("GetVersion");
            var args = new Dictionary<string, object>
            {
                { "RepositoryPath", repositoryPath }
            };

            var result = (string)this.session.ExecuteScalar(sp, args, CommandType.StoredProcedure);
            return string.IsNullOrEmpty(result) ? "0" : result;
        }

        public string GetHash(string scriptName)
        {
            var sp = this.FullyQualifiedObjectName("GetCurrentScriptHash");
            var args = new Dictionary<string, object>
            {
                { "ScriptName", scriptName }
            };

            return (string)this.session.ExecuteScalar(sp, args, CommandType.StoredProcedure);
        }

        private static string GetIdentity()
        {
            var current = WindowsIdentity.GetCurrent();
            return current != null ? current.Name : string.Empty;
        }

        private string FullyQualifiedObjectName(string name)
        {
            return $"[{this.systemDatabase}].[{this.systemSchema}].{name}";
        }
    }
}
