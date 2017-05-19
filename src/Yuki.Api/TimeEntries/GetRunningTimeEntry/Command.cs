namespace Yuki.Api.TimeEntries.GetRunningTimeEntry
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
                var current = this.repository.GetCurrent(req.UserId);

                // Clients might not always be aware of any running timer
                // so they might request one just to see if there is one.
                // Our mapping profile will fail when we feed it a `null` 
                // value so in this case we just return an empty response.
                if(current == null)
                {
                    return Some<Response, Exception>(
                        new Response());
                }

                var data = Mapper.Map<IDictionary<string, object>>(current);
                var response = new Response { Data = data };
                return Some<Response, Exception>(response);
            }
            catch (Exception ex)
            {
                return None<Response, Exception>(ex);
            }
        }
    }
}