
using CampusSurveyManagementSystem.Application.Common.Models;
using CampusSurveyManagementSystem.Application.Responses.DTOs;

namespace CampusSurveyManagementSystem.Application.Responses.Interfaces;

public interface IResponseService
{
    Task<Result<Guid>> SubmitAsync( SubmitResponseRequest request,   CancellationToken cancellationToken = default);
}