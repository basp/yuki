namespace Yuki.Templates
{
    using System;
    using System.Diagnostics.Contracts;
    using Optional;
    using Optional.Linq;
    using SmartFormat;

    public class CreateDatabaseTemplate
    {
        private static readonly string ResourceName =
            $"{nameof(Yuki)}.Resources.{nameof(CreateDatabaseTemplate)}.sql";

        public CreateDatabaseTemplate(string database)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(database));
            this.Database = database;
        }

        public string Database
        {
            get;
            private set;
        }

        public Option<string, Exception> Format()
        {
            var tmpl = typeof(CreateDatabaseTemplate)
                .Assembly
                .ReadEmbeddedString(ResourceName);

            return from s in tmpl
                   select Smart.Format(s, this);
        }
    }
}