namespace Yuki.Api.Clients
{
    using System;
    using System.Web.Http;
    using Yuki.Data;

    [RoutePrefix("api/clients")]
    // [Route("api/v{version:apiVersion}/[controller]")]
    public class ClientsController : ApiController
    {
        private readonly Repository repository;

        public ClientsController(Repository repository)
        {
            this.repository = repository;
        }

        [HttpPost]
        [Route(Name = nameof(CreateClient))]
        public IHttpActionResult CreateClient(
            [FromBody] CreateClient.Request request)
        {
            var cmd = new CreateClient.Command(this.repository);
            var res = cmd.Execute(request);
            return res.Match<IHttpActionResult>(
                some => this.Json(some),
                none => this.InternalServerError(none));
        }

        [HttpDelete]
        [Route("{clientId}", Name = nameof(DeleteClient))]
        public IHttpActionResult DeleteClient(
            [FromUri] int clientId)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("{clientId}", Name = nameof(GetClientDetails))]
        public IHttpActionResult GetClientDetails(
            [FromUri] int clientId)
        {
            var cmd = new GetClientDetails.Command(this.repository);
            var req = new GetClientDetails.Request { ClientId = clientId };
            var res = cmd.Execute(req);
            return res.Match<IHttpActionResult>(
                some => this.Json(some),
                none => this.InternalServerError(none));
        }

        [HttpGet]
        [Route("{clientId}/projects", Name = nameof(GetClientProjects))]
        public IHttpActionResult GetClientProjects(
            [FromUri] int clientId)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route(Name = nameof(GetUserVisibleClients))]
        public IHttpActionResult GetUserVisibleClients()
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        [Route("{clientId}", Name = nameof(UpdateClient))]
        public IHttpActionResult UpdateClient(
            [FromUri] int clientId,
            [FromBody] UpdateClient.Request request)
        {
            request.ClientId = clientId;
            var cmd = new UpdateClient.Command(this.repository);
            var res = cmd.Execute(request);
            return res.Match<IHttpActionResult>(
                some => this.Json(some),
                none => this.InternalServerError(none));
        }
    }
}