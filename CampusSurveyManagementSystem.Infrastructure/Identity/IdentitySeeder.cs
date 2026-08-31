
using CampusSurveyManagementSystem.Application.Common.Authorization;
using CampusSurveyManagementSystem.Application.Identity.Constants;
using Microsoft.AspNetCore.Identity;

namespace CampusSurveyManagementSystem.Infrastructure.Identity;

public static class IdentitySeeder
{
    public static async Task SeedRolesAsync( RoleManager<IdentityRole<Guid>> roleManager)
    {
        var roles = new[]
        {
            Roles.SuperAdmin,
            Roles.OrganizationAdmin,
            Roles.SurveyManager,
            Roles.Respondent
        };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(
                    new IdentityRole<Guid>
                    {
                        Id = Guid.NewGuid(),
                        Name = role,
                        NormalizedName = role.ToUpperInvariant()
                    });
            }
        }
    }
}