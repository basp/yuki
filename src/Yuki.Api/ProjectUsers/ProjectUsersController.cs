namespace Yuki.Api.ProjectUsers
{
    using System;
    using System.Web.Http;

    [RoutePrefix("api/project_users")]
    public class ProjectUsersController : ApiController
    {
        [HttpPost]
        [Route]
        public IHttpActionResult CreateProjectUser(
            [FromBody] dynamic request)
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        [Route("{projectUserId}")]
        public IHttpActionResult DeleteProjectUser(
            [FromUri] int projectUserId)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Route("{projectUserId}")]
        public IHttpActionResult UpdateProjectUser(
            [FromUri] int projectUserId,
            [FromBody] dynamic request)
        {
            throw new NotImplementedException();
        }
    }
}