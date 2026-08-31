
using CampusSurveyManagementSystem.Application.Common.Interfaces;
using CampusSurveyManagementSystem.Application.Common.Models;
using CampusSurveyManagementSystem.Application.Organizations.Interfaces;
using CampusSurveyManagementSystem.Domain.Organizations;
using Microsoft.EntityFrameworkCore;

namespace CampusSurveyManagementSystem.Application.Organizations.Services;

public class OrganizationMembershipService : IOrganizationMembershipService
{
    private readonly IApplicationDbContext _context;

    public OrganizationMembershipService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> AddMemberAsync(Guid organizationId, Guid userId, OrganizationRole role,   CancellationToken cancellationToken = default)
    {
        if (organizationId == Guid.Empty)
        {
            return Result.Failure( "Organization is required.",   ErrorType.Validation);
        }

        if (userId == Guid.Empty)
        {
            return Result.Failure( "User is required.",  ErrorType.Validation);
        }

        var organizationExists = await _context.Organizations
            .AnyAsync( x => x.Id == organizationId,  cancellationToken);

        if (!organizationExists)
        {
            return Result.Failure( "Organization was not found.",   ErrorType.NotFound);
        }

        var existingMembership = await _context.OrganizationMemberships
            .FirstOrDefaultAsync( x =>
                    x.OrganizationId == organizationId && x.UserId == userId, cancellationToken);

        if (existingMembership is not null)
        {
            if (existingMembership.IsActive)
            {
                return Result.Failure(
                    "User is already a member of this organization.",
                    ErrorType.Conflict);
            }

            existingMembership.Activate();

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        var membership = new OrganizationMembership( organizationId, userId, role);

        _context.OrganizationMemberships.Add(membership);

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }


    public async Task<Result> RemoveMemberAsync(Guid organizationId, Guid userId,  CancellationToken cancellationToken = default)
    {
        var membership = await _context.OrganizationMemberships
            .FirstOrDefaultAsync(x =>
                    x.OrganizationId == organizationId &&
                    x.UserId == userId,
                cancellationToken);

        if (membership is null)
        {
            return Result.Failure("Organization membership was not found.", ErrorType.NotFound);
        }

        if (!membership.IsActive)
        {
            return Result.Failure( "User is already inactive in this organization.", ErrorType.Conflict);
        }

        membership.Deactivate();

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }


    public async Task<Result> ReactivateMemberAsync(
        Guid organizationId,
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var membership = await _context.OrganizationMemberships
            .FirstOrDefaultAsync(
                x =>
                    x.OrganizationId == organizationId &&
                    x.UserId == userId,
                cancellationToken);

        if (membership is null)
        {
            return Result.Failure(
                "Organization membership was not found.",
                ErrorType.NotFound);
        }

        if (membership.IsActive)
        {
            return Result.Failure(
                "User is already an active member of this organization.",
                ErrorType.Conflict);
        }

        membership.Activate();

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }


    public async Task<bool> IsMemberAsync( Guid organizationId, Guid userId, CancellationToken cancellationToken = default)
    {
        if (organizationId == Guid.Empty ||
            userId == Guid.Empty)
        {
            return false;
        }

        return await _context.OrganizationMemberships
            .AsNoTracking()
            .AnyAsync(
                x =>
                    x.OrganizationId == organizationId &&
                    x.UserId == userId &&
                    x.IsActive,
                cancellationToken);
    }
}