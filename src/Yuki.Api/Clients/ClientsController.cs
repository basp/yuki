namespace Yuki.Api.Clients
{
    using System;
    using System.Web.Http;

    [RoutePrefix("api/v1/clients")]
    public class ClientsController : ApiController
    {
        [HttpPost]
        [Route(Name = nameof(CreateClient))]
        public IHttpActionResult CreateClient(
            [FromBody] CreateClient.Request request)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }
    }
}