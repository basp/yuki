namespace Yuki.Api.TimeEntries.GetTimeEntries
{
    using System;
    using System.Collections.Generic;
    using AutoMapper;
    using Optional;
    using Yuki.Data;

    using static Optional.Option;

    public class Command : ICommand<Request, Response, Exception>
    {
        private readonly TimeEntryRepository repository;

        public Command(TimeEntryRepository repository)
        {
            this.repository = repository;
        }

        public Option<Response, Exception> Execute(Request req)
        {
            try
            {
                var entries = this.repository.GetEntries(
                    req.StartDate,
                    req.StartDate);

                var res = new Response
                {
                    Items = MapEntries(entries),
                };

                return Some<Response, Exception>(res);
            }
            catch (Exception ex)
            {
                return None<Response, Exception>(ex);
            }
        }

        private static IEnumerable<IDictionary<string, object>> MapEntries(
            IEnumerable<TimeEntry> entries) =>
            Mapper.Map<IEnumerable<IDictionary<string, object>>>(entries);
    }
}