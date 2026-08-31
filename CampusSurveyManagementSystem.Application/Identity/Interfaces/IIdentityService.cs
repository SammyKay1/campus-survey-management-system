

using CampusSurveyManagementSystem.Application.Common.Models;
using CampusSurveyManagementSystem.Application.Identity.DTOs;

namespace CampusSurveyManagementSystem.Application.Identity.Interfaces;

public interface IIdentityService
{
    Task<Result<UserDto>> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default);

    Task<Result<LoginResultDto>> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);

    Task<Result> LogoutAsync( CancellationToken cancellationToken = default);



}

