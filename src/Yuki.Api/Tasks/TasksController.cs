namespace Yuki.Api.Tasks
{
    using System;
    using System.Web.Http;

    [RoutePrefix("api/tasks")]
    public class TasksController : ApiController
    {
        [HttpPost]
        [Route]
        public IHttpActionResult CreateTask(
            [FromBody] dynamic request)
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        [Route("{taskId}")]
        public IHttpActionResult DeleteTask(
            [FromUri] int taskId)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("{taskId}")]
        public IHttpActionResult GetTaskDetails(
            [FromUri] int taskId)
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        [Route("{taskId}")]
        public IHttpActionResult UpdateTask(
            [FromUri] int taskId,
            [FromBody] dynamic request)
        {
            throw new NotImplementedException();
        }
    }
}