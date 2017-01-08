namespace Yuki
{
    using System;
    using Templates;

    public class TextTemplateFactory : ITextTemplateFactory
    {
        public ITextTemplate GetCreateDatabaseTemplate(string database)
        {
            return new CreateDatabaseTemplate(database);
        }

        public ITextTemplate GetCreateRepositoryTemplate(
            string repositoryDatabase,
            string repositorySchema)
        {
            return new CreateRepositoryTemplate(
                repositoryDatabase,
                repositorySchema);
        }

        public ITextTemplate GetDropDatabaseTemplate(string database)
        {
            return new DropDatabaseTemplate(database);
        }

        public ITextTemplate GetRestoreDatabaseTemplate(string database)
        {
            return new RestoreDatabaseTemplate(database);
        }
    }
}
