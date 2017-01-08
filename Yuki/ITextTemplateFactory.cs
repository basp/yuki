namespace Yuki
{
    public interface ITextTemplateFactory
    {
        ITextTemplate GetCreateDatabaseTemplate(
            string database);

        ITextTemplate GetCreateRepositoryTemplate(
            string repositoryDatabase,
            string repositorySchema);

        ITextTemplate GetDropDatabaseTemplate(
            string database);

        ITextTemplate GetRestoreDatabaseTemplate(
            string database);
    }
}
