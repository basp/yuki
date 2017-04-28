namespace Yuki.Api.Workspaces
{
    using System;
    using System.Web.Http;
    using Yuki.Model;

    [RoutePrefix("api/v1/workspaces")]
    public class WorkspacesController : ApiController
    {
        private readonly Repository repository;

        public WorkspacesController(Repository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        [Route("{workspaceId}")]
        public IHttpActionResult GetSingleWorkspace(
            [FromUri] int workspaceId)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("{workspaceId}/clients")]
        public IHttpActionResult GetWorkspaceClients(
            [FromUri] int workspaceId)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("{workspaceId}/groups")]
        public IHttpActionResult GetWorkspaceGroups(
            [FromUri] int workspaceId)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("{workspaceId}/projects")]
        public IHttpActionResult GetWorkspaceProjects(
            [FromUri] int workspaceId)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route]
        public IHttpActionResult GetWorkspaces()
        {
            throw new NotImplementedException();
        }
        [HttpGet]
        [Route("{workspaceId}/tags")]
        public IHttpActionResult GetWorkspaceTags(
            [FromUri] int workspaceId)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("{workspaceId}/tasks")]
        public IHttpActionResult GetWorkspaceTasks(
            [FromUri] int workspaceId)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("{workspaceId}/users")]
        public IHttpActionResult GetWorkspaceUsers(
            [FromUri] int workspaceId)
        {
            throw new NotImplementedException();
        }
    }
}