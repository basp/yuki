namespace Yuki.Templates
{
    using System;
    using System.Diagnostics.Contracts;
    using Optional;
    using Optional.Linq;
    using SmartFormat;

    public class CreateRepositoryTemplate
    {
        private static string resourceName =
            $"{nameof(Yuki)}.Resources.{nameof(CreateRepositoryTemplate)}.sql";

        public CreateRepositoryTemplate(
            string repositoryDatabase,
            string repositorySchema)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(repositoryDatabase));
            Contract.Requires(!string.IsNullOrWhiteSpace(repositorySchema));

            this.RepositoryDatabase = repositoryDatabase;
            this.RepositorySchema = repositorySchema;
        }

        public string RepositoryDatabase
        {
            get;
            private set;
        }

        public string RepositorySchema
        {
            get;
            private set;
        }

        public Option<string, Exception> Format()
        {
            var tmpl = typeof(CreateRepositoryTemplate)
                .Assembly
                .ReadEmbeddedString(resourceName);

            return from fmt in tmpl
                   select Smart.Format(fmt, this);
        }
    }
}