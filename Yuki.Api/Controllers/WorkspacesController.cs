﻿namespace Yuki.Api.Controllers
{
    using System;
    using System.Web.Http;
    using Yuki.Model;

    [RoutePrefix("api/workspaces")]
    public class WorkspacesController : ApiController
    {
        private readonly Repository repository;

        public WorkspacesController(Repository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        [Route]
        public IHttpActionResult Get()
        {
            var workspaces = this.repository.GetWorkspaces();
            return this.Json(workspaces);
        }

        [HttpGet]
        [Route("{workspaceId}", Name = "GetWorkspace")]
        public IHttpActionResult Get(int workspaceId)
        {
            var workspace = this.repository.GetWorkspace(workspaceId);
            return workspace == null
                ? (IHttpActionResult)this.NotFound()
                : this.Json(workspace);
        }

        [HttpPost]
        public IHttpActionResult NewWorkspace(Workspace workspace)
        {
            this.repository.InsertWorkspace(workspace);
            var routeValues = new { workspaceId = workspace.Id };
            var location = this.Url.Route("GetWorkspace", routeValues);
            return this.Created(location, workspace);
        }
    }
}