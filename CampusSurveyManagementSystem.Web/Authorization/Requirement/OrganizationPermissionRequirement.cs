
using CampusSurveyManagementSystem.Domain.Organizations;
using Microsoft.AspNetCore.Authorization;

namespace CampusSurveyManagementSystem.Web.Authorization.Requirements;

public sealed class OrganizationPermissionRequirement  : IAuthorizationRequirement
{
    public OrganizationPermission Permission { get; }

    public OrganizationPermissionRequirement( OrganizationPermission permission)
    {
        Permission = permission;
    }
}