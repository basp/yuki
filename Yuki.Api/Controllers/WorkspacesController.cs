namespace Yuki.Api.Controllers
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
    }
}