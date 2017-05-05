namespace Yuki.Api.TimeEntries.CreateTimeEntry
{
    using System;
    using System.Collections.Generic;
    using AutoMapper;
    using Optional;
    using Yuki.Data;

    using static Optional.Option;

    public class Command : ICommand<Request, Response, Exception>
    {
        private readonly TimeEntryRepository timeEntryRepository;
        private readonly Repository<Workspace> workspaceRepository;

        public Command(
            TimeEntryRepository timeEntryRepository,
            Repository<Workspace> workspaceRepository)
        {
            this.timeEntryRepository = timeEntryRepository;
            this.workspaceRepository = workspaceRepository;
        }

        public Option<Response, Exception> Execute(Request req)
        {
            try
            {
                var timeEntry = Mapper.Map<TimeEntry>(req.TimeEntry);

                if (timeEntry.TaskId.HasValue)
                {
                    throw new NotImplementedException();
                }
                else if (timeEntry.ProjectId.HasValue)
                {
                    throw new NotImplementedException();
                }

                this.timeEntryRepository.Insert(timeEntry);
                var data = Mapper.Map<IDictionary<string, object>>(timeEntry);
                return Some<Response, Exception>(new Response(data));
            }
            catch (Exception ex)
            {
                return None<Response, Exception>(ex);
            }
        }
    }
}