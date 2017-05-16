namespace Yuki.Api.Projects
{
    using System;
    using System.Web.Http;
    using Yuki.Data;

    [RoutePrefix("api/projects")]
    public class ProjectsController : ApiController
    {
        private readonly Repository<Project> repository;

        public ProjectsController(Repository<Project> repository)
        {
            this.repository = repository;
        }

        [HttpPost]
        [Route(Name = nameof(CreateProject))]
        public IHttpActionResult CreateProject(
            [FromBody] CreateProject.Request request)
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        [Route("{projectId}", Name = nameof(DeleteProject))]
        public IHttpActionResult DeleteProject(
            [FromUri] int projectId)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("{projectId}", Name = nameof(GetProject))]
        public IHttpActionResult GetProject(
            [FromUri] int projectId)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("{projectId}/tasks", Name = nameof(GetProjectTasks))]
        public IHttpActionResult GetProjectTasks(
            [FromUri] int projectId)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("{projectId}/project_users", Name = nameof(GetProjectUsers))]
        public IHttpActionResult GetProjectUsers(
            [FromUri] int projectId)
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        [Route("{projectId}", Name = nameof(UpdateProject))]
        public IHttpActionResult UpdateProject(
            [FromUri] int projectId,
            [FromBody] UpdateProject.Request request)
        {
            throw new NotImplementedException();
        }
    }
}