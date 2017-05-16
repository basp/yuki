namespace Yuki.Api.Users
{
    using System;
    using System.Web.Http;

    [RoutePrefix("api/me")]
    public class UsersController : ApiController
    {
        [HttpGet]
        [Route]
        public IHttpActionResult GetCurrentUserData()
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        public IHttpActionResult UpdateUser(
            [FromBody] dynamic request)
        {
            throw new NotImplementedException();
        }
    }
}