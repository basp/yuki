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

    using Ex = SqlRepositoryException;

    public class SqlRepository : IRepository<int, Ex>
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

        public Option<bool, Ex> Initialize()
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

                return Some<bool, Ex>(true);
            }
            catch (Exception ex)
            {
                var msg = $"Could not initialize repository in '[{this.config.RepositoryDatabase}].[{this.config.RepositorySchema}]'.";
                var error = new Ex(msg, ex);
                return None<bool, Ex>(error);
            }
        }

        public Option<int, Ex> InsertVersion(
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
                return Some<int, Ex>(result);
            }
            catch (Exception ex)
            {
                var msg = $"Could not insert version.";
                var error = new Ex(msg, ex);
                return None<int, Ex>(error);
            }
        }

        public Option<int, Ex> InsertScriptRun(
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
                return Some<int, Ex>(result);
            }
            catch (Exception ex)
            {
                var msg = $"Could not insert script run.";
                var error = new Ex(msg, ex);
                return None<int, Ex>(error);
            }
        }

        public Option<int, Ex> InsertScriptRunError(
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
                return Some<int, Ex>(result);
            }
            catch (Exception ex)
            {
                var msg = $"Could not insert script run error.";
                var error = new Ex(msg, ex);
                return None<int, Ex>(error);
            }
        }

        public Option<string, Ex> GetHash(
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
                return Some<string, Ex>(result);
            }
            catch (Exception ex)
            {
                var msg = $"Could not get script hash.";
                var error = new Ex(msg, ex);
                return None<string, Ex>(error);
            }
        }

        public Option<string, Ex> GetVersion(
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
                return Some<string, Ex>(result)
                    .Map(x => string.IsNullOrWhiteSpace(x) ? "0" : x);
            }
            catch (Exception ex)
            {
                var msg = $"Could not get version.";
                var error = new Ex(msg, ex);
                return None<string, Ex>(error);
            }
        }

        public Option<bool, Ex> HasScriptRunAlready(
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
                return Some<int, Ex>(result).Map(x => x > 0);
            }
            catch (Exception ex)
            {
                var msg = $"Could not get script run status.";
                var error = new Ex(msg, ex);
                return None<bool, Ex>(error);
            }
        }

        private string FullyQualified(string name) =>
            $"[{this.config.RepositoryDatabase}].[{this.config.RepositorySchema}].{name}";
    }
}