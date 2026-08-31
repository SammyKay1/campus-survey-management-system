
using CampusSurveyManagementSystem.Application.Common.Models;
using CampusSurveyManagementSystem.Domain.Organizations;

namespace CampusSurveyManagementSystem.Application.Organizations.Interfaces;

public interface IOrganizationMembershipService
{
    Task<Result> AddMemberAsync(Guid organizationId, Guid userId, OrganizationRole role,  CancellationToken cancellationToken = default);

    Task<Result> RemoveMemberAsync(Guid organizationId, Guid userId,  CancellationToken cancellationToken = default);

    Task<Result> ReactivateMemberAsync(Guid organizationId,  Guid userId, CancellationToken cancellationToken = default);

    Task<bool> IsMemberAsync(Guid organizationId,  Guid userId, CancellationToken cancellationToken = default);
}