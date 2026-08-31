
using CampusSurveyManagementSystem.Domain.Organizations;

namespace CampusSurveyManagementSystem.Application.Organizations.Interfaces;

public interface IOrganizationAccessService
{
    Task<bool> HasPermissionAsync(Guid userId,   Guid organizationId, OrganizationPermission permission,  CancellationToken cancellationToken = default);
}