namespace Yuki.Api.Controllers
{
    using System;
    using System.Web.Http;
    using Yuki.Model;

    [RoutePrefix("api/users")]
    public class UsersController : ApiController
    {
        private readonly Repository repository;

        public UsersController(Repository repository)
        {
            this.repository = repository;
        }

        [Route("{userId}", Name = WellKnownRoutes.GetUser)]
        public IHttpActionResult Get(
            [FromUri] int userId)
        {
            var user = this.repository.GetUser(userId);
            return user == null
                ? (IHttpActionResult)this.NotFound()
                : this.Json(user);
        }
    }
}