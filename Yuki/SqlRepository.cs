namespace Yuki
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using NLog;
    using Optional;
    using SmartFormat;

    using static Optional.Option;

    public class SqlRepository : IRepository<int, Exception>
    {
        private static readonly string CreateRepositoryTemplate =
            $"{nameof(Yuki)}.Resources.{nameof(CreateRepositoryTemplate)}.sql";

        private readonly ILogger log = LogManager.GetCurrentClassLogger();
        private readonly ISession session;
        private readonly ISqlRepositoryConfig config;

        public SqlRepository(
            ISession session,
            ISqlRepositoryConfig config)
        {
            Contract.Requires(session != null);
            Contract.Requires(session.Connection.State == ConnectionState.Open);
            Contract.Requires(config != null);

            this.session = session;
            this.config = config;
        }

        public Option<bool, Exception> Initialize()
        {
            try
            {
                var asm = typeof(SqlRepository).Assembly;
                var tmpl = asm.ReadEmbeddedString(CreateRepositoryTemplate);
                var cmdText = Smart.Format(tmpl, this.config);
                var stmts = StatementSplitter.Split(cmdText).ToList();

                foreach (var stmt in stmts)
                {
                    this.session.ExecuteNonQuery(
                        stmt,
                        new Dictionary<string, object>(),
                        CommandType.Text);
                }

                return Some<bool, Exception>(true);
            }
            catch (Exception ex)
            {
                var msg = $"Could not initialize repository in '[{this.config.RepositoryDatabase}].[{this.config.RepositorySchema}]'.";
                var error = new Exception(msg, ex);
                return None<bool, Exception>(error);
            }
        }

        public Option<int, Exception> InsertVersion(
            IVersionRecord record)
        {
            try
            {
                var sp = this.FullyQualified("InsertVersion");
                var args = new Dictionary<string, object>()
                {
                    ["VersionName"] = record.VersionName,
                    ["RepositoryPath"] = record.RepositoryPath,
                    ["EnteredBy"] = record.EnteredBy,
                };

                var result = (int)this.session.ExecuteScalar(sp, args, CommandType.StoredProcedure);
                return Some<int, Exception>(result);
            }
            catch (Exception ex)
            {
                var msg = $"Could not insert version.";
                var error = new Exception(msg, ex);
                return None<int, Exception>(error);
            }
        }

        public Option<int, Exception> InsertScriptRun(
            IScriptRunRecord<int> record)
        {
            try
            {
                var sp = this.FullyQualified("InsertScriptRun");
                var args = new Dictionary<string, object>()
                {
                    ["VersionId"] = record.VersionId,
                    ["ScriptName"] = record.ScriptName,
                    ["TextOfScript"] = record.Sql,
                    ["TextHash"] = record.Hash,
                    ["OneTimeScript"] = record.IsOneTimeScript,
                    ["EnteredBy"] = record.EnteredBy,
                };

                var result = (int)this.session.ExecuteScalar(sp, args, CommandType.StoredProcedure);
                return Some<int, Exception>(result);
            }
            catch (Exception ex)
            {
                var msg = $"Could not insert script run.";
                var error = new Exception(msg, ex);
                return None<int, Exception>(error);
            }
        }

        public Option<int, Exception> InsertScriptRunError(
            IScriptRunErrorRecord record)
        {
            try
            {
                var sp = this.FullyQualified("InsertScriptRunError");
                var args = new Dictionary<string, object>()
                {
                    ["RepositoryPath"] = record.RepositoryPath,
                    ["ScriptName"] = record.ScriptName,
                    ["VersionName"] = record.RepositoryVersion,
                    ["TextOfScript"] = record.Sql,
                    ["ErroneousPart"] = record.SqlErrorPart,
                    ["ErrorMessage"] = record.ErrorMessage,
                    ["EnteredBy"] = record.EnteredBy,
                };

                var result = (int)this.session.ExecuteScalar(sp, args, CommandType.StoredProcedure);
                return Some<int, Exception>(result);
            }
            catch (Exception ex)
            {
                var msg = $"Could not insert script run error.";
                var error = new Exception(msg, ex);
                return None<int, Exception>(error);
            }
        }

        public Option<string, Exception> GetHash(
            string scriptName)
        {
            try
            {
                var sp = this.FullyQualified("GetHash");
                var args = new Dictionary<string, object>()
                {
                    ["ScriptName"] = scriptName,
                };

                var result = (string)this.session.ExecuteScalar(sp, args, CommandType.StoredProcedure);
                return Some<string, Exception>(result);
            }
            catch (Exception ex)
            {
                var msg = $"Could not get script hash.";
                var error = new Exception(msg, ex);
                return None<string, Exception>(error);
            }
        }

        public Option<string, Exception> GetVersion(
            string repositoryPath)
        {
            try
            {
                var sp = this.FullyQualified("GetVersion");
                var args = new Dictionary<string, object>()
                {
                    ["RepositoryPath"] = repositoryPath,
                };

                var result = (string)this.session.ExecuteScalar(sp, args, CommandType.StoredProcedure);
                return Some<string, Exception>(result)
                    .Map(x => string.IsNullOrWhiteSpace(x) ? "0" : x);
            }
            catch (Exception ex)
            {
                var msg = $"Could not get version.";
                var error = new Exception(msg, ex);
                return None<string, Exception>(error);
            }
        }

        public Option<bool, Exception> HasScriptRunAlready(
            string scriptName)
        {
            try
            {
                var sp = this.FullyQualified("HasScriptRunAlready");
                var args = new Dictionary<string, object>()
                {
                    ["ScriptName"] = scriptName,
                };

                var result = (int)this.session.ExecuteScalar(sp, args, CommandType.StoredProcedure);
                return Some<int, Exception>(result).Map(x => x > 0);
            }
            catch (Exception ex)
            {
                var msg = $"Could not get script run status.";
                var error = new Exception(msg, ex);
                return None<bool, Exception>(error);
            }
        }

        private string FullyQualified(string name) =>
            $"[{this.config.RepositoryDatabase}].[{this.config.RepositorySchema}].{name}";
    }
}