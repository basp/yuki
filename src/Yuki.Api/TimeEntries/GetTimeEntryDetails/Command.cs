namespace Yuki.Api.TimeEntries.GetTimeEntryDetails
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
                var entry = this.repository.GetById(req.TimeEntryId);
                var data = Mapper.Map<IDictionary<string, object>>(entry);
                return Some<Response, Exception>(new Response(data));
            }
            catch (Exception ex)
            {
                return None<Response, Exception>(ex);
            }
        }
    }
}