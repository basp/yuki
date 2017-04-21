namespace Yuki.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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

        [HttpGet]
        [Route]
        public IHttpActionResult Get()
        {
            var users = this.repository.GetAllUsers();
            var model = CreateViewModel(users);
            return this.Json(model);
        }

        [HttpGet]
        [Route("{userId}", Name = WellKnownRoutes.GetUser)]
        public IHttpActionResult Get(
            [FromUri] int userId)
        {
            var user = this.repository.GetUser(userId);
            if (user == null)
            {
                return this.NotFound();
            }

            var model = CreateViewModel(user);
            return this.Json(model);
        }

        private static IEnumerable<object> CreateViewModel(IEnumerable<User> users) =>
            users.Select(CreateViewModel).ToList();

        private static object CreateViewModel(User user) =>
            new
            {
                user.Name,
                user.Email,
                Entries = user.Entries.Select(x => new
                {
                    x.Id,
                    x.Description,
                    x.Duration,
                }),
            };
    }
}