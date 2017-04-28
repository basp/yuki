namespace Yuki.Api.Groups
{
    using System;
    using System.Web.Http;

    [RoutePrefix("api/v1/groups")]
    public class GroupsController : ApiController
    {
        [HttpPost]
        [Route]
        public IHttpActionResult CreateGroup(
            [FromBody] dynamic request)
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        [Route("{groupId}")]
        public IHttpActionResult DeleteGroup(
            [FromUri] int groupId)
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        [Route("{groupId}")]
        public IHttpActionResult UpdateGroup(
            [FromUri] int groupId,
            [FromBody] dynamic request)
        {
            throw new NotImplementedException();
        }
    }
}