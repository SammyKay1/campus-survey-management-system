
using System.Security.Claims;
using CampusSurveyManagementSystem.Domain.Responses;
using Microsoft.AspNetCore.Authorization;

namespace CampusSurveyManagementSystem.Web.Authorization.Requirements;

public sealed class ResponseOwnerAuthorizationHandler
    : AuthorizationHandler<ResponseOwnerRequirement, SurveyResponse>
{
    protected override Task HandleRequirementAsync( AuthorizationHandlerContext context, ResponseOwnerRequirement requirement,
        SurveyResponse resource)
    {
        var userIdValue = context.User.FindFirstValue( ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userIdValue, out var userId))
        {
            return Task.CompletedTask;
        }

        if (resource.RespondentUserId == userId)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}