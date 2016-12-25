namespace Yuki.Templates
{
    using System;
    using System.Diagnostics.Contracts;
    using Optional;
    using Optional.Linq;
    using SmartFormat;

    public class DropDatabaseTemplate
    {
        private static readonly string ResourceName =
            $"{nameof(Yuki)}.Resources.{nameof(DropDatabaseTemplate)}.sql";

        public DropDatabaseTemplate(string database)
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
            var tmpl = typeof(DropDatabaseTemplate)
                .Assembly
                .ReadEmbeddedString(ResourceName);

            return from fmt in tmpl
                   select Smart.Format(fmt, this);
        }
    }
}