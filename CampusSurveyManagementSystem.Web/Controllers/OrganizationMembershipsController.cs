
using CampusSurveyManagementSystem.Application.Organizations.Interfaces;
using CampusSurveyManagementSystem.Domain.Organizations;
using CampusSurveyManagementSystem.Web.Common;
using Microsoft.AspNetCore.Mvc;

namespace CampusSurveyManagementSystem.Web.Controllers;

[ApiController]
[Route("api/organizations/{organizationId:guid}/members")]
public class OrganizationMembershipsController : ControllerBase
{
    private readonly IOrganizationMembershipService _membershipService;

    public OrganizationMembershipsController( IOrganizationMembershipService membershipService)
    {
        _membershipService = membershipService;
    }


    [HttpPost("{userId:guid}")]
    public async Task<IActionResult> AddMember( Guid organizationId,  Guid userId, OrganizationRole role,  CancellationToken cancellationToken = default)
    {
        var result = await _membershipService.AddMemberAsync( organizationId, userId, role ,cancellationToken);

        return result.ToActionResult(this);
    }


    [HttpDelete("{userId:guid}")]
    public async Task<IActionResult> RemoveMember( Guid organizationId,  Guid userId, CancellationToken cancellationToken = default)
    {
        var result = await _membershipService.RemoveMemberAsync(organizationId, userId,  cancellationToken);

        return result.ToActionResult(this);
    }


    [HttpPatch("{userId:guid}/reactivate")]
    public async Task<IActionResult> ReactivateMember(Guid organizationId, Guid userId,    CancellationToken cancellationToken = default)
    {
        var result = await _membershipService.ReactivateMemberAsync(
            organizationId,
            userId,
            cancellationToken);

        return result.ToActionResult(this);
    }
}