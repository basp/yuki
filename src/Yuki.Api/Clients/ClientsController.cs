namespace Yuki.Api.Controllers
{
    using System;
    using System.Web.Http;

    [RoutePrefix("api/v1/clients")]
    public class ClientsController : ApiController
    {
        [HttpPost]
        [Route]
        public IHttpActionResult CreateClient(
            [FromBody] dynamic request)
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        [Route("{clientId}")]
        public IHttpActionResult DeleteClient(
            [FromUri] int clientId)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("{clientId}")]
        public IHttpActionResult GetClientDetails(
            [FromUri] int clientId)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("{clientId}/projects")]
        public IHttpActionResult GetClientProjects(
            [FromUri] int clientId)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route]
        public IHttpActionResult GetUserVisibleClients()
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        [Route("{clientId}")]
        public IHttpActionResult UpdateClient(
            [FromUri] int clientId,
            [FromBody] dynamic request)
        {
            throw new NotImplementedException();
        }
    }
}