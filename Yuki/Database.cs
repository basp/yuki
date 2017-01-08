namespace Yuki
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.Contracts;
    using Optional;
    using Optional.Linq;

    public class Database : IDatabase
    {
        private readonly ISession session;
        private readonly ITextTemplateFactory textTemplateFactory;
        private readonly string name;

        public Database(
            ISession session,
            ITextTemplateFactory textTemplateFactory,
            string name)
        {
            Contract.Requires(session != null);
            Contract.Requires(textTemplateFactory != null);
            Contract.Requires(!string.IsNullOrWhiteSpace(name));
            this.session = session;
            this.textTemplateFactory = textTemplateFactory;
            this.name = name;
        }

        public Option<bool, Exception> Create()
        {
            var tmpl = this.textTemplateFactory
                .GetCreateDatabaseTemplate(this.name);

            return from cmdText in tmpl.Format()
                   let args = new Dictionary<string, object>()
                   from res in this.session.TryExecuteScalar<bool>(
                       cmdText,
                       args,
                       CommandType.Text)
                   select res;
        }

        public Option<int, Exception> Drop()
        {
            var tmpl = this.textTemplateFactory
                .GetDropDatabaseTemplate(this.name);

            return from cmdText in tmpl.Format()
                   let args = new Dictionary<string, object>()
                   from res in this.session.TryExecuteNonQuery(
                       cmdText,
                       args,
                       CommandType.Text)
                   select res;
        }

        public Option<bool, Exception> Restore(string backup)
        {
            var tmpl = this.textTemplateFactory
                .GetRestoreDatabaseTemplate(this.name);

            return from cmdText in tmpl.Format()
                   let args = new Dictionary<string, object>
                   {
                       ["Backup"] = backup,
                   }
                   from res in this.session.TryExecuteNonQuery(
                       cmdText,
                       args,
                       CommandType.Text)
                   select true;
        }
    }
}
