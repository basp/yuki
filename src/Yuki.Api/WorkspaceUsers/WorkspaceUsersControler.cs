namespace Yuki.Api.WorkspaceUsers
{
    using System;
    using System.Web.Http;

    [RoutePrefix("api/workspaces")]
    public class WorkspaceUsersControler : ApiController
    {
        [HttpPost]
        [Route("{workspaceId}/invite")]
        public IHttpActionResult InviteUsers(
            [FromUri] int workspaceId, 
            [FromBody] dynamic request)
        {
            throw new NotImplementedException();
        }
    }
}