
using System.Security.Claims;
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
        var userIdClaim =   context.User.FindFirstValue( ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userIdClaim, out var userId))
        {
            return;
        }

        var hasPermission =
            await _organizationAccessService.HasPermissionAsync(
                userId,  resource.OrganizationId,   requirement.Permission);

        if (hasPermission)
        {
            context.Succeed(requirement);
        }
    }
}