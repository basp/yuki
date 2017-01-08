namespace Yuki
{
    using System;
    using System.Diagnostics.Contracts;

    public class DatabaseFactory : IDatabaseFactory
    {
        private readonly ISession session;
        private readonly ITextTemplateFactory textTemplateFactory;

        public DatabaseFactory(
            ISession session,
            ITextTemplateFactory textTemplateFactory)
        {
            Contract.Requires(session != null);
            Contract.Requires(textTemplateFactory != null);
            this.session = session;
            this.textTemplateFactory = textTemplateFactory;
        }

        public IDatabase Create(string name)
        {
            return new Database(
                this.session,
                this.textTemplateFactory,
                name);
        }
    }
}
