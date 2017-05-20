namespace Yuki.Api
{
    using System.Security.Claims;
    using System.Web.Http;

    public static class Extensions
    {
        public static Data.User GetUser(
            this ApiController controller,
            Data.UserRepository userRepository)
        {
            var caller = controller.User as ClaimsPrincipal;
            var subject = caller.FindFirst("sub");
            return userRepository.GetBySubject(subject.Value);
        }
    }
}