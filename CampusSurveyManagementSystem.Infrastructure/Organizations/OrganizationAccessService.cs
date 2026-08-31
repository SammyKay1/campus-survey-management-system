
using CampusSurveyManagementSystem.Application.Organizations.Interfaces;
using CampusSurveyManagementSystem.Domain.Organizations;
using CampusSurveyManagementSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CampusSurveyManagementSystem.Infrastructure.Organizations;

public class OrganizationAccessService : IOrganizationAccessService
{
    private readonly ApplicationDbContext _context;

    public OrganizationAccessService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> HasPermissionAsync(Guid userId,  Guid organizationId,  OrganizationPermission permission,
        CancellationToken cancellationToken = default)
    {
        var membership = await _context.OrganizationMemberships
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x =>
                    x.UserId == userId &&
                    x.OrganizationId == organizationId &&
                    x.IsActive,
                cancellationToken);

        if (membership is null)
        {
            return false;
        }

        return permission switch
        {
            OrganizationPermission.ViewSurvey =>
                membership.Role is
                    OrganizationRole.Analyst or
                    OrganizationRole.SurveyManager or
                    OrganizationRole.OrganizationAdmin,

            OrganizationPermission.ManageSurvey =>
                membership.Role is
                    OrganizationRole.SurveyManager or
                    OrganizationRole.OrganizationAdmin,

            OrganizationPermission.PublishSurvey =>
                membership.Role is
                    OrganizationRole.SurveyManager or
                    OrganizationRole.OrganizationAdmin,

            OrganizationPermission.ViewResponses =>
                membership.Role is
                    OrganizationRole.SurveyManager or
                    OrganizationRole.OrganizationAdmin,

            OrganizationPermission.ExportResponses =>
                membership.Role is
                    OrganizationRole.OrganizationAdmin,

            _ => false
        };
    }
}