namespace Yuki.Api
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using IdentityServer3.Core.Extensions;
    using IdentityServer3.Core.Models;
    using IdentityServer3.Core.Services.Default;

    public class UserService : UserServiceBase
    {
        private readonly Data.UserRepository userRepository;

        public UserService(Data.UserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public override Task AuthenticateLocalAsync(
            LocalAuthenticationContext context)
        {
            var user = this.userRepository.Authenticate(
                context.UserName,
                context.Password);

            if(user != null)
            {
                context.AuthenticateResult =
                    new AuthenticateResult(user.Subject, user.Username);
            }

            return Task.FromResult(0);
        }

        public override Task GetProfileDataAsync(
            ProfileDataRequestContext context)
        {
            var user = this.userRepository.GetBySubject(
                context.Subject.GetSubjectId());

            if(user != null)
            {
                context.IssuedClaims = user
                    .GetClaims()
                    .Where(x => context.RequestedClaimTypes.Contains(x.Type));
            }

            return Task.FromResult(0);
        }
    }
}