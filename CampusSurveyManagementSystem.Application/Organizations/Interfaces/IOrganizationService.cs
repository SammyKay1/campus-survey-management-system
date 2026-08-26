
using CampusSurveyManagementSystem.Application.Common.Models;
using CampusSurveyManagementSystem.Application.Organizations.DTOs;

namespace CampusSurveyManagementSystem.Application.Organizations.Interfaces;

public interface IOrganizationService
{
    Task<Result<OrganizationDto>> GetByIdAsync( Guid id,  CancellationToken cancellationToken = default);

    Task<Result<PagedResult<OrganizationDto>>> GetAllAsync( int pageNumber = 1,  int pageSize = 20, CancellationToken cancellationToken = default);

    Task<Result<OrganizationDto>> CreateAsync( CreateOrganizationRequest request,  CancellationToken cancellationToken = default);

    Task<Result> UpdateAsync( Guid id,  UpdateOrganizationRequest request,  CancellationToken cancellationToken = default);

    Task<Result> ActivateAsync( Guid id,  CancellationToken cancellationToken = default);

    Task<Result> DeactivateAsync( Guid id,  CancellationToken cancellationToken = default);
}