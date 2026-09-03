
using System.Security.Claims;
using CampusSurveyManagementSystem.Application.Common.Interfaces;
using CampusSurveyManagementSystem.Domain.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace CampusSurveyManagementSystem.Web.Authorization.Handlers;

public sealed class ResponseAccessAuthorizationHandler
    : AuthorizationHandler<ResponseAccessRequirement, SurveyResponse>
{
    private readonly IApplicationDbContext _context;

    public ResponseAccessAuthorizationHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        ResponseAccessRequirement requirement,
        SurveyResponse resource)
    {
        var userIdValue = context.User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userIdValue, out var userId))
        {
            return;
        }

        // ============================================================
        // 1. Respondent ownership
        // ============================================================

        if (resource.RespondentUserId == userId)
        {
            context.Succeed(requirement);
            return;
        }

        // ============================================================
        // 2. Organization-level administrative access
        // ============================================================

        var organizationId = await _context.Surveys
            .Where(x => x.Id == resource.SurveyId)
            .Select(x => x.OrganizationId)
            .FirstOrDefaultAsync();

        if (organizationId == Guid.Empty)
        {
            return;
        }

        var membership = await _context.OrganizationMemberships
            .FirstOrDefaultAsync( x =>   x.OrganizationId == organizationId &&  x.UserId == userId && x.IsActive);

        if (membership is null)
        {
            return;
        }

        if (membership.Role == Domain.Organizations.OrganizationRole.OrganizationAdmin)
        {
            context.Succeed(requirement);
        }
    }
}