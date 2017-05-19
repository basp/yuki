namespace Yuki.Api.Users
{
    using System;
    using System.Web.Http;
    using Yuki.Data;

    [RoutePrefix("api/me")]
    public class UsersController : ApiController
    {
        private UserRepository userRepository;

        public UsersController(UserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        [HttpGet]
        [Route]
        public IHttpActionResult GetCurrentUserData()
        {
            var user = this.userRepository.GetBySubject("basp@yuki.com");
            var data = new
            {
                user.Email,
                user.FullName,
                user.LastUpdated,
            };

            return this.Json(new { data });
        }

        [HttpPut]
        public IHttpActionResult UpdateUser(
            [FromBody] dynamic request)
        {
            throw new NotImplementedException();
        }
    }
}