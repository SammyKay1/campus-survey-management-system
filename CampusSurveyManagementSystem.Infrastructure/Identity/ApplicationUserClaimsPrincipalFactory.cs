
using System.Security.Claims;
//using CampusSurveyManagementSystem.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using CampusSurveyManagementSystem.Application.Identity.Constants;


namespace CampusSurveyManagementSystem.Infrastructure.Identity;

public class ApplicationUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, IdentityRole<Guid>>
{
    public ApplicationUserClaimsPrincipalFactory( UserManager<ApplicationUser> userManager,  RoleManager<IdentityRole<Guid>> roleManager,
        IOptions<IdentityOptions> optionsAccessor)
        : base(userManager, roleManager, optionsAccessor)
    {
    }

    protected override async Task<ClaimsIdentity> GenerateClaimsAsync( ApplicationUser user)
    {
        var identity = await base.GenerateClaimsAsync(user);

        identity.AddClaim( new Claim(IdentityClaimTypes.UserId, user.Id.ToString()));

        identity.AddClaim( new Claim(IdentityClaimTypes.FirstName, user.FirstName.ToString()));

        identity.AddClaim( new Claim(IdentityClaimTypes.LastName, user.LastName.ToString()));

        return identity;
    }
}