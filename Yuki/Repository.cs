namespace Yuki
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.Contracts;
    using Optional;
    using Optional.Linq;

    using static Utils;

    public class Repository : IRepository
    {
        private readonly ISession session;
        private readonly ITextTemplateFactory textTemplateFactory;
        private readonly string repositoryDatabase;
        private readonly string repositorySchema;

        public Repository(
            ISession session,
            ITextTemplateFactory textTemplateFactory,
            string repositoryDatabase,
            string repositorySchema)
        {
            Contract.Requires(session != null);
            Contract.Requires(textTemplateFactory != null);
            Contract.Requires(!string.IsNullOrWhiteSpace(repositoryDatabase));
            Contract.Requires(!string.IsNullOrWhiteSpace(repositorySchema));

            this.session = session;
            this.textTemplateFactory = textTemplateFactory;
            this.repositoryDatabase = repositoryDatabase;
            this.repositorySchema = repositorySchema;
        }

        public Option<bool, Exception> Initialize()
        {
            var tmpl = this.textTemplateFactory.GetCreateRepositoryTemplate(
                this.repositoryDatabase,
                this.repositorySchema);

            return from sql in tmpl.Format()
                   let stmts = StatementSplitter.Split(sql)
                   from res in this.ExecuteStatements(stmts)
                   select res;
        }

        public Option<string, Exception> GetCurrentHash(string scriptName)
        {
            var sp = FullyQualifiedObjectName(
                this.repositoryDatabase,
                this.repositorySchema,
                "GetCurrentScriptHash");

            var args = new Dictionary<string, object>
            {
                ["ScriptName"] = scriptName,
            };

            return this.session.TryExecuteScalar<string>(
                sp,
                args,
                CommandType.StoredProcedure);
        }

        public Option<bool, Exception> HasScriptRun(string scriptName)
        {
            var sp = FullyQualifiedObjectName(
               this.repositoryDatabase,
               this.repositorySchema,
               "HasScriptRunAlready");

            var args = new Dictionary<string, object>
            {
                ["ScriptName"] = scriptName,
            };

            var count = this.session.TryExecuteScalar<int>(
                sp,
                args,
                CommandType.StoredProcedure);

            return count.Map(x => x > 0);
        }

        public Option<string, Exception> GetVersion(string repositoryPath)
        {
            var sp = FullyQualifiedObjectName(
                this.repositoryDatabase,
                this.repositorySchema,
                "GetVersion");

            var args = new Dictionary<string, object>
            {
                ["RepositoryPath"] = repositoryPath,
            };

            return this.session.TryExecuteScalar<string>(
                sp,
                args,
                CommandType.StoredProcedure);
        }

        private Option<bool, Exception> ExecuteStatements(
            IEnumerable<string> stmts)
        {
            try
            {
                foreach (var s in stmts)
                {
                    var args = new Dictionary<string, object>();
                    var ct = CommandType.Text;
                    this.session.ExecuteNonQuery(s, args, ct);
                }

                return Option.Some<bool, Exception>(true);
            }
            catch (Exception ex)
            {
                return Option.None<bool, Exception>(ex);
            }
        }
    }
}
