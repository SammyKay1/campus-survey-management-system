using System.Security.Claims;
using CampusSurveyManagementSystem.Application.Organizations.Interfaces;
using CampusSurveyManagementSystem.Domain.Common;
using Microsoft.AspNetCore.Authorization;

namespace CampusSurveyManagementSystem.Web.Authorization;

public class OrganizationAccessHandler : AuthorizationHandler<OrganizationAccessRequirement, IOrganizationResource>
{
    private readonly IOrganizationMembershipService _membershipService;

    public OrganizationAccessHandler(IOrganizationMembershipService membershipService)
    {
        _membershipService = membershipService;
    }

    protected override async Task HandleRequirementAsync( AuthorizationHandlerContext context,   OrganizationAccessRequirement requirement,
    IOrganizationResource resource)
    {
        if (resource is null)
        {
            return;
        }

        var userIdClaim = context.User.FindFirstValue( ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userIdClaim, out var userId))
        {
            return;
        }

        var isMember = await _membershipService.IsMemberAsync( resource.OrganizationId,   userId);

        if (isMember)
        {
            context.Succeed(requirement);
        }
    }
}