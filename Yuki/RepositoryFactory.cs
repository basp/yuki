namespace Yuki
{
    using System.Diagnostics.Contracts;

    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly ISession session;
        private readonly ITextTemplateFactory textTemplateFactory;

        public RepositoryFactory(
            ISession session,
            ITextTemplateFactory textTemplateFactory)
        {
            Contract.Requires(session != null);
            Contract.Requires(textTemplateFactory != null);

            this.session = session;
            this.textTemplateFactory = textTemplateFactory;
        }

        public IRepository Create(
            string repositoryDatabase,
            string repositorySchema)
        {
            return new Repository(
                this.session,
                this.textTemplateFactory,
                repositoryDatabase,
                repositorySchema);
        }
    }
}
