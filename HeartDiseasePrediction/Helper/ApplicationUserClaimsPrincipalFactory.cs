using Database.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HeartDiseasePrediction.Helper
{
    public class ApplicationUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>
    {
        public ApplicationUserClaimsPrincipalFactory(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,
            IOptions<IdentityOptions> options) : base(userManager, roleManager, options)
        {

        }
        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            identity.AddClaim(new Claim("UserFirstName", user.FirstName ?? ""));
            identity.AddClaim(new Claim("UserLastName", user.LastName ?? ""));
            identity.AddClaim(new Claim("UserAge", user.Age.ToString() ?? ""));
            identity.AddClaim(new Claim("UserPhoneNumber", user.PhoneNumber ?? ""));
            identity.AddClaim(new Claim("UserBirthDate", user.BirthDate.ToString() ?? ""));
            identity.AddClaim(new Claim("UserGender", user.Gender.ToString() ?? ""));
            identity.AddClaim(new Claim("UserEmail", user.Email ?? ""));
            identity.AddClaim(new Claim("UserProfileImage", user.ProfileImg ?? ""));
            return identity;
        }
    }
}
