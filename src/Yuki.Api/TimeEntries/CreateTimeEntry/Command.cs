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
                var @cur = this.timeEntryRepository.GetCurrent(req.UserId);
                if (@cur != null)
                {
                    var msg = "There's already a timer running.";
                    var ex = new Exception(msg);
                    return None<Response, Exception>(ex);
                }

                var @new = Mapper.Map<TimeEntry>(req.TimeEntry);
                if (@new.TaskId.HasValue)
                {
                    throw new NotImplementedException();
                }
                else if (@new.ProjectId.HasValue)
                {
                    throw new NotImplementedException();
                }

                this.timeEntryRepository.Insert(@new);
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