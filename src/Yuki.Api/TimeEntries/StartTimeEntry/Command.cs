namespace Yuki.Api.TimeEntries.StartTimeEntry
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
                var @cur = this.repository.GetCurrent(req.UserId);
                if (@cur != null)
                {
                    var ex = new CurrentTimerException();
                    return None<Response, Exception>(ex);
                }

                req.TimeEntry[F.Start] = DateTime.UtcNow.ToString();

                var @new = Mapper.Map<TimeEntry>(req.TimeEntry);
                @new.UserId = req.UserId;

                this.repository.Insert(@new);
                var data = Mapper.Map<IDictionary<string, object>>(@new);
                return Some<Response, Exception>(new Response(data));
            }
            catch (Exception ex)
            {
                return None<Response, Exception>(ex);
            }
        }
    }
}