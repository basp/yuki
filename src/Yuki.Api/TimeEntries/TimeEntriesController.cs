﻿namespace Yuki.Api.TimeEntries
{
    using System;
    using System.Web.Http;
    using Yuki.Data;

    [RoutePrefix("api/time_entries")]
    public class TimeEntriesController : ApiController
    {
        private const int TestUserId = 1;

        private readonly TimeEntryRepository timeEntryRepository;
        private readonly Repository<Workspace> workspaceRepository;

        public TimeEntriesController(
            TimeEntryRepository timeEntryRepository,
            Repository<Workspace> workspaceRepository)
        {
            this.timeEntryRepository = timeEntryRepository;
            this.workspaceRepository = workspaceRepository;
        }

        [HttpPost]
        [Route(Name = nameof(CreateTimeEntry))]
        public IHttpActionResult CreateTimeEntry(
            [FromBody] CreateTimeEntry.Request req)
        {
            var cmd = new CreateTimeEntry.Command(
                this.timeEntryRepository,
                this.workspaceRepository);

            var res = cmd.Execute(req.WithUserId(TestUserId));
            return res.Match(
                some => (IHttpActionResult)this.Json(some),
                none => this.InternalServerError(none));
        }

        [HttpDelete]
        [Route("{timeEntryId}", Name = nameof(DeleteTimeEntry))]
        public IHttpActionResult DeleteTimeEntry(
            [FromUri] int timeEntryId)
        {
            var cmd = new DeleteTimeEntry.Command(
                this.timeEntryRepository);

            var res = cmd.Execute(
                new DeleteTimeEntry.Request(timeEntryId));

            return res.Match(
                some => (IHttpActionResult)this.Ok(),
                none => this.InternalServerError(none));
        }

        [HttpGet]
        [Route("current", Name = nameof(GetCurrentTimeEntry))]
        public IHttpActionResult GetCurrentTimeEntry()
        {
            var cmd = new GetRunningTimeEntry.Command(
                this.timeEntryRepository);

            var res = cmd.Execute(
                new GetRunningTimeEntry.Request(TestUserId));

            return res.Match(
                some => (IHttpActionResult)this.Json(some),
                none => this.NotFound());
        }

        [HttpGet]
        [Route("{timeEntryId}", Name = nameof(GetTimeEntry))]
        public IHttpActionResult GetTimeEntry(
            [FromUri] int timeEntryId)
        {
            var cmd = new GetTimeEntryDetails.Command(
                this.timeEntryRepository);

            var res = cmd.Execute(
                new GetTimeEntryDetails.Request(timeEntryId));

            return res.Match(
                some => (IHttpActionResult)this.Json(some),
                none => this.InternalServerError(none));
        }

        [HttpGet]
        [Route(Name = nameof(GetTimeEntries))]
        public IHttpActionResult GetTimeEntries(
            [FromUri] DateTime? startDate,
            [FromUri] DateTime? endDate)
        {
            endDate = endDate.HasValue
                ? endDate
                : DateTime.UtcNow;

            startDate = startDate.HasValue
                ? startDate
                : endDate.Value.Subtract(TimeSpan.FromDays(9));

            var cmd = new GetTimeEntries.Command(this.timeEntryRepository);
            var res = cmd.Execute(new GetTimeEntries.Request(
                startDate.Value,
                endDate.Value).WithUserId(TestUserId));

            return res.Match(
                some => (IHttpActionResult)this.Json(some),
                none => this.InternalServerError(none));
        }

        [HttpPost]
        [Route("start", Name = nameof(StartTimeEntry))]
        public IHttpActionResult StartTimeEntry(
            [FromBody] StartTimeEntry.Request request)
        {
            var cmd = new StartTimeEntry.Command(this.timeEntryRepository);
            var res = cmd.Execute(request.WithUserId(TestUserId));

            return res.Match(
                some => (IHttpActionResult)this.Json(some),
                none => this.InternalServerError(none));
        }

        [HttpPut]
        [Route("{timeEntryId}/stop", Name = nameof(StopTimeEntry))]
        public IHttpActionResult StopTimeEntry(
            [FromUri] int timeEntryId)
        {
            var cmd = new StopTimeEntry.Command(this.timeEntryRepository);
            var res = cmd.Execute(new StopTimeEntry.Request(timeEntryId));

            return res.Match(
                some => (IHttpActionResult)this.Json(some),
                none => this.InternalServerError(none));
        }

        [HttpPut]
        [Route("{timeEntryId}", Name = nameof(UpdateTimeEntry))]
        public IHttpActionResult UpdateTimeEntry(
            [FromUri] int timeEntryId,
            [FromBody] UpdateTimeEntry.Request request)
        {
            throw new NotImplementedException();
        }
    }
}