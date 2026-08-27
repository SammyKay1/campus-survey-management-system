
using CampusSurveyManagementSystem.Application.Common.Interfaces;
using CampusSurveyManagementSystem.Application.Common.Models;
using CampusSurveyManagementSystem.Application.Organizations.DTOs;
using CampusSurveyManagementSystem.Application.Organizations.Interfaces;
using CampusSurveyManagementSystem.Domain.Organizations;
using Microsoft.EntityFrameworkCore;

namespace CampusSurveyManagementSystem.Application.Organizations.Services;

public class OrganizationService : IOrganizationService
{
    private readonly IApplicationDbContext _context;

    public OrganizationService( IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<OrganizationDto>> GetByIdAsync(  Guid id,  CancellationToken cancellationToken = default)
    {
        var organization = await _context.Organizations.AsNoTracking()
            .FirstOrDefaultAsync(  x => x.Id == id, cancellationToken);

        if (organization is null)
        {
            return Result<OrganizationDto>.Failure( "Organization was not found.");
        }

        return Result<OrganizationDto>.Success( MapToDto(organization));
    }

    public async Task<Result<PagedResult<OrganizationDto>>> GetAllAsync( int pageNumber = 1,  int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        if (pageNumber < 1)
        {
            return Result<PagedResult<OrganizationDto>>.Failure( "Page number must be greater than zero.");
        }

        if (pageSize < 1 || pageSize > 100)
        {
            return Result<PagedResult<OrganizationDto>>.Failure( "Page size must be between 1 and 100.");
        }

        var query = _context.Organizations.AsNoTracking().OrderBy(x => x.Name);

        var totalCount = await query.CountAsync(cancellationToken);

        var organizations = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

        var items = organizations.Select(MapToDto).ToArray();

        var result = new PagedResult<OrganizationDto>
        {
            Items = items,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };

        return Result<PagedResult<OrganizationDto>>.Success( result);
    }

    public async Task<Result<OrganizationDto>> CreateAsync( CreateOrganizationRequest request, CancellationToken cancellationToken = default)
    {
        var codeExists = await _context.Organizations.AnyAsync( x => x.Code == request.Code, cancellationToken);

        if (codeExists)
        {
            return Result<OrganizationDto>.Failure( "An organization with this code already exists.");
        }

        var organization = new Domain.Organizations.Organization( request.Name,  request.Code,  request.Description);

        _context.Organizations.Add(organization);

        await _context.SaveChangesAsync(cancellationToken);

        return Result<OrganizationDto>.Success( MapToDto(organization));
    }

    public async Task<Result> UpdateAsync(Guid id,  UpdateOrganizationRequest request, CancellationToken cancellationToken = default)
    {
        var organization = await _context.Organizations
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);

        if (organization is null)
        {
            return Result.Failure(
                "Organization was not found.");
        }

        organization.Update(
            request.Name,
            request.Description);

        await _context.SaveChangesAsync(
            cancellationToken);

        return Result.Success();
    }

    public async Task<Result> ActivateAsync( Guid id,  CancellationToken cancellationToken = default)
    {
        var organization = await _context.Organizations
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (organization is null)
        {
            return Result.Failure( "Organization was not found.");
        }

        organization.Activate();

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> DeactivateAsync(Guid id,  CancellationToken cancellationToken = default)
    {
        var organization = await _context.Organizations .FirstOrDefaultAsync( x => x.Id == id, cancellationToken);

        if (organization is null)
        {
            return Result.Failure( "Organization was not found.");
        }

        organization.Deactivate();

        await _context.SaveChangesAsync( cancellationToken);

        return Result.Success();
    }

    private static OrganizationDto MapToDto( Organization organization)
    {
        return new OrganizationDto
        {
            Id = organization.Id,
            Name = organization.Name,
            Code = organization.Code,
            Description = organization.Description,
            IsActive = organization.IsActive,
            CreatedAt = organization.CreatedAt
        };
    }
}