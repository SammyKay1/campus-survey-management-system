
using System.Security.Claims;
using CampusSurveyManagementSystem.Application.Identity.Constants;
using CampusSurveyManagementSystem.Application.Organizations.Interfaces;
using CampusSurveyManagementSystem.Domain.Organizations;
using CampusSurveyManagementSystem.Domain.Surveys;
using CampusSurveyManagementSystem.Web.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;

namespace CampusSurveyManagementSystem.Web.Authorization.Handlers;

public sealed class OrganizationResourceAuthorizationHandler  : AuthorizationHandler< OrganizationPermissionRequirement, Survey>
{
    private readonly IOrganizationAccessService _organizationAccessService;

    public OrganizationResourceAuthorizationHandler( IOrganizationAccessService organizationAccessService)
    {
        _organizationAccessService = organizationAccessService;
    }

    protected override async Task HandleRequirementAsync( AuthorizationHandlerContext context,
        OrganizationPermissionRequirement requirement,   Survey resource)
    {
        var userIdClaim =   context.User.FindFirstValue( IdentityClaimTypes.UserId);

         Console.WriteLine("=== AUTHORIZATION DEBUG ===");
    Console.WriteLine($"Authenticated: {context.User.Identity?.IsAuthenticated}");
    Console.WriteLine($"UserId claim: {userIdClaim}");
    Console.WriteLine($"Required permission: {requirement.Permission}");
    Console.WriteLine($"Survey ID: {resource.Id}");
    Console.WriteLine($"Organization ID: {resource.OrganizationId}");

        if (!Guid.TryParse(userIdClaim, out var userId))
        {
            Console.WriteLine("Failed: Could not parse UserId claim to Guid.");
            return;
        }

        var hasPermission =  await _organizationAccessService.HasPermissionAsync( userId,  resource.OrganizationId,  
                             requirement.Permission);

                             Console.WriteLine($"Has permission: {hasPermission}");

        if (hasPermission)
        {
            context.Succeed(requirement);

        }
    }
}