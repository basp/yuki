namespace Yuki.Templates
{
    using System;
    using System.Diagnostics.Contracts;
    using Optional;
    using Optional.Linq;
    using SmartFormat;

    public class RestoreDatabaseTemplate : ITextTemplate
    {
        private static readonly string ResourceName =
            $"{nameof(Yuki)}.Resources.{nameof(RestoreDatabaseTemplate)}.sql";

        public RestoreDatabaseTemplate(string database)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(database));

            this.Database = database;
        }

        public string Database
        {
            get;
        }

        public Option<string, Exception> Format()
        {
            var tmpl = typeof(RestoreDatabaseTemplate)
                .Assembly
                .ReadEmbeddedString(ResourceName);

            return from fmt in tmpl
                   select Smart.Format(fmt, this);
        }
    }
}