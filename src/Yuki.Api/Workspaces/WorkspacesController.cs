namespace Yuki.Api.Workspaces
{
    using System;
    using System.Web.Http;
    using Yuki.Data;

    [RoutePrefix("api/workspaces")]
    public class WorkspacesController : ApiController
    {
        private readonly Repository<Workspace> workspaceRepository;
        private readonly WorkspaceUserRepository workspaceUserRepository;

        public WorkspacesController(
            Repository<Workspace> workspaceRepository,
            WorkspaceUserRepository workspaceUserRepository)
        {
            this.workspaceRepository = workspaceRepository;
            this.workspaceUserRepository = workspaceUserRepository;
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
            var cmd = new GetWorkspaces.Command(
                this.workspaceRepository);

            var res = cmd.Execute(new GetWorkspaces.Request(KnownIds.TestUser));
            return res.Match(
                some => (IHttpActionResult)this.Json(some),
                none => this.InternalServerError(none));
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