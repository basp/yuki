namespace Yuki.Api.Projects
{
    using System;
    using System.Web.Http;

    [RoutePrefix("api/v1/projects")]
    public class ProjectsController : ApiController
    {
        [HttpPost]
        [Route]
        public IHttpActionResult CreateProject(
            [FromBody] dynamic request)
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        [Route("{projectId}")]
        public IHttpActionResult DeleteProject(
            [FromUri] int projectId)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("{projectId}")]
        public IHttpActionResult GetProject(
            [FromUri] int projectId)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("{projectId}/tasks")]
        public IHttpActionResult GetProjectTasks(
            [FromUri] int projectId)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("{projectId}/project_users")]
        public IHttpActionResult GetProjectUsers(
            [FromUri] int projectId)
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        [Route("{projectId}")]
        public IHttpActionResult UpdateProject(
            [FromUri] int projectId,
            [FromBody] dynamic request)
        {
            throw new NotImplementedException();
        }
    }
}