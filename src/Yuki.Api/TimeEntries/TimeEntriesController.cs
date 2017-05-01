namespace Yuki.Api.TimeEntries
{
    using System;
    using System.Web.Http;

    [RoutePrefix("api/time_entries")]
    public class TimeEntriesController : ApiController
    {
        [HttpPost]
        [Route]
        public IHttpActionResult CreateTimeEntry()
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        [Route("{timeEntryId}")]
        public IHttpActionResult DeleteTimeEntry(
            [FromUri] int timeEntryId)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("current")]
        public IHttpActionResult GetCurrentTimeEntry()
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("{timeEntryId}")]
        public IHttpActionResult GetTimeEntry(
            [FromUri] int timeEntryId)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Route("{timeEntryId}/start")]
        public IHttpActionResult StartTimeEntry(
            [FromUri] int timeEntryId,
            [FromBody] dynamic request)
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        [Route("{timeEntryId}/stop")]
        public IHttpActionResult StopTimeEntry(
            [FromUri] int timeEntryId)
        {
            throw new NotImplementedException();
        }
        [HttpPut]
        [Route("{timeEntryId}")]
        public IHttpActionResult UpdateTimeEntry(
            [FromUri] int timeEntryId,
            [FromBody] dynamic request)
        {
            throw new NotImplementedException();
        }
    }
}