namespace Yuki
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using Dapper;
    using NLog;
    using Optional;
    using SmartFormat;
   
    public class SqlRepository : IRepository<int, SqlRepositoryException>
    {
        private static string CreateRepositoryTemplate =
            $"{nameof(Yuki)}.Resources.CreateRepository.sql";

        private readonly ILogger log = LogManager.GetCurrentClassLogger();
        private readonly ISession session;
        private readonly IIdentityProvider identity;
        private readonly ISqlRepositoryConfig config;

        public SqlRepository(
            ISession session,
            IIdentityProvider identity,
            ISqlRepositoryConfig config)
        {
            Contract.Requires(session != null);
            Contract.Requires(session.Connection.State == ConnectionState.Open);
            Contract.Requires(identity != null);
            Contract.Requires(config != null);

            this.session = session;
            this.identity = identity;
            this.config = config;
        }

        public Option<bool, SqlRepositoryException> Initialize()
        {
            try
            {
                var asm = typeof(SqlRepository).Assembly;
                var tmpl = asm.ReadEmbeddedString(CreateRepositoryTemplate);
                var cmdText = Smart.Format(tmpl, this.config);
                var stmts = StatementSplitter.Split(cmdText).ToList();

                this.log.Debug($"Got {stmts.Count} statement(s) to execute after splitting");
               
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
                var msg = $"Could not initialize repository in '[{this.config.Database}].[{this.config.Schema}]'.";
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
