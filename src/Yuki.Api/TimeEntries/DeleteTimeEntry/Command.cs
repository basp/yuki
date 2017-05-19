namespace Yuki.Api.TimeEntries.DeleteTimeEntry
{
    using System;
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
                var timeEntry = this.repository.GetById(req.TimeEntryId);
                this.repository.Delete(timeEntry);
                return Some<Response, Exception>(new Response());
            }
            catch (Exception ex)
            {
                return None<Response, Exception>(ex);
            }
        }
    }
}