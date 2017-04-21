namespace Yuki.Api.Controllers
{
    using System;
    using System.Web.Http;
    using Yuki.Model;

    [RoutePrefix("api/timers")]
    public class TimersController : ApiController
    {
        private readonly Repository repository;

        public TimersController(Repository repository)
        {
            this.repository = repository;
        }

        [Route("{timerId}", Name = WellKnownRoutes.GetTimer)]
        public IHttpActionResult Get(
            [FromUri] int timerId)
        {
            var timer = this.repository.GetTimer(timerId);
            return timer == null
                ? (IHttpActionResult)this.NotFound()
                : this.Json(timer);
        }

        [HttpPost]
        [Route("start")]
        public IHttpActionResult Start(
            [FromUri] int workspaceId, 
            [FromUri] int userId)
        {
            Timer timer;

            timer = this.repository.GetUserTimer(userId);
            if (timer != null)
            {
                return this.Ok(timer);
            }

            timer = new Timer(workspaceId, userId);
            this.repository.InsertTimer(timer);

            var routeValues = new { timerId = timer.Id };
            var location = this.Url.Route("GetTimer", routeValues);
            return this.Created(location, timer);
        }

        [HttpPost]
        [Route("stop")]
        public IHttpActionResult Stop(
            [FromUri] int timerId)
        {
            var timer = this.repository.GetTimer(timerId);
            if (timer == null)
            {
                return this.NotFound();
            }

            var entry = new Entry
            {
                UserId = timer.UserId,
                WorkspaceId = timer.WorkspaceId,
                Description = timer.Description,
                Duration = DateTime.UtcNow.Subtract(timer.Started),
            };

            this.repository.InsertEntry(entry);
            this.repository.DeleteTimer(timer);

            var routeValues = new { entryId = entry.Id };
            var location = this.Url.Route("GetEntry", routeValues);
            return this.Created(location, entry);
        }

        [HttpPut]
        public IHttpActionResult UpdateTimer(
            [FromBody] Timer timer)
        {
            var existing = this.repository.GetTimer(timer.Id);
            if(existing == null)
            {
                return this.NotFound();
            }

            this.repository.UpdateTimer(timer);
            return this.Ok();
        }
    }
}