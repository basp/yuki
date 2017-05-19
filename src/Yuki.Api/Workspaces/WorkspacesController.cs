namespace Yuki.Api.Workspaces
{
    using System;
    using System.Web.Http;
    using Yuki.Data;

    [RoutePrefix("api/workspaces")]
    public class WorkspacesController : ApiController
    {
        private readonly WorkspaceRepository workspaceRepository;
        private readonly WorkspaceUserRepository workspaceUserRepository;

        public WorkspacesController(
            WorkspaceRepository workspaceRepository,
            WorkspaceUserRepository workspaceUserRepository)
        {
            this.workspaceRepository = workspaceRepository;
            this.workspaceUserRepository = workspaceUserRepository;
        }

        [HttpGet]
        [Route("{workspaceId}", Name = nameof(GetSingleWorkspace))]
        public IHttpActionResult GetSingleWorkspace(
            [FromUri] int workspaceId)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("{workspaceId}/clients", Name = nameof(GetWorkspaceClients))]
        public IHttpActionResult GetWorkspaceClients(
            [FromUri] int workspaceId)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("{workspaceId}/groups", Name = nameof(GetWorkspaceGroups))]
        public IHttpActionResult GetWorkspaceGroups(
            [FromUri] int workspaceId)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("{workspaceId}/projects", Name = nameof(GetWorkspaceProjects))]
        public IHttpActionResult GetWorkspaceProjects(
            [FromUri] int workspaceId)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route(Name = nameof(GetWorkspaces))]
        public IHttpActionResult GetWorkspaces()
        {
            var cmd = new GetWorkspaces.Command(
                this.workspaceRepository,
                this.workspaceUserRepository);

            var res = cmd.Execute(new GetWorkspaces.Request(KnownIds.TestUser));
            return res.Match(
                some => (IHttpActionResult)this.Json(some.Items),
                none => this.InternalServerError(none));
        }

        [HttpGet]
        [Route("{workspaceId}/tags", Name = nameof(GetWorkspaceTags))]
        public IHttpActionResult GetWorkspaceTags(
            [FromUri] int workspaceId)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("{workspaceId}/tasks", Name = nameof(GetWorkspaceTasks))]
        public IHttpActionResult GetWorkspaceTasks(
            [FromUri] int workspaceId)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("{workspaceId}/users", Name = nameof(GetWorkspaceUsers))]
        public IHttpActionResult GetWorkspaceUsers(
            [FromUri] int workspaceId)
        {
            throw new NotImplementedException();
        }
    }
}