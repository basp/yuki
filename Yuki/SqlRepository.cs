namespace Yuki
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.Contracts;
    using Dapper;
    using NLog;
    using Optional;
    using SmartFormat;
    using System.Security.Principal;

    public class SqlRepository : IRepository<int, SqlRepositoryException>
    {
        private static string CreateRepositoryTemplate =
            $"{nameof(Yuki)}.Resources.CreateRepository.sql";

        private readonly ILogger log = LogManager.GetCurrentClassLogger();
        private readonly ISession session;
        private readonly IIdentityProvider identity;

        public SqlRepository(
            ISession session,
            IIdentityProvider identity)
        {
            Contract.Requires(session != null);
            Contract.Requires(session.Connection.State == ConnectionState.Open);
            Contract.Requires(identity != null);
            Contract.Requires(identity != null);

            this.session = session;
            this.identity = identity;
        }

        public Option<bool, SqlRepositoryException> Initialize(
            string database,
            string schema)
        {
            try
            {
                var asm = typeof(SqlRepository).Assembly;
                var tmpl = asm.ReadEmbeddedString(CreateRepositoryTemplate);
                var args = new
                {
                    Database = database,
                    Schema = schema
                };

                var cmdText = Smart.Format(tmpl, args);
                var stmts = StatementSplitter.Split(cmdText);
                foreach (var stmt in stmts)
                {
                    this.session.ExecuteNonQuery(
                        stmt,
                        new Dictionary<string, object>(),
                        CommandType.Text);
                }

                return Option.Some<bool, SqlRepositoryException>(true);
            }
            catch (Exception ex)
            {
                var msg = $"Could not initialize repository in '[{database}].[{schema}]'.";
                var error = new SqlRepositoryException(msg, ex);
                return Option.None<bool, SqlRepositoryException>(error);
            }
        }

        public Option<int, SqlRepositoryException> InsertVersion(
            VersionRecord record)
        {
            
            
            throw new NotImplementedException();
        }

        public Option<int, SqlRepositoryException> InsertScriptRun(
            ScriptRunRecord<int> record)
        {
            throw new NotImplementedException();
        }

        public Option<int, SqlRepositoryException> InsertScriptRunError(
            ScriptRunErrorRecord record)
        {
            throw new NotImplementedException();
        }

        public Option<string, SqlRepositoryException> GetHash(
            string scriptName)
        {
            throw new NotImplementedException();
        }

        public Option<string, SqlRepositoryException> GetVersion(
            string repositoryPath)
        {
            throw new NotImplementedException();
        }

        public Option<bool, SqlRepositoryException> HasScriptRunAlready(
            string scriptName)
        {
            throw new NotImplementedException();
        }
    }
}
