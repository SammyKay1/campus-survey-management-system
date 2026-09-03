using CampusSurveyManagementSystem.Application.Common.Models;
using CampusSurveyManagementSystem.Application.Surveys.DTOs;
using CampusSurveyManagementSystem.Domain.Responses;
using CampusSurveyManagementSystem.Domain.Surveys;

namespace CampusSurveyManagementSystem.Application.Surveys.Interfaces;

public interface ISurveyService
{
    Task<Result<SurveyDto>> GetByIdAsync( Guid id,  CancellationToken cancellationToken = default);

    Task<Result<PagedResult<SurveyDto>>> GetAllAsync( int pageNumber = 1, int pageSize = 20,  CancellationToken cancellationToken = default);

    Task<Result<SurveyDto>> CreateAsync( CreateSurveyRequest request,    CancellationToken cancellationToken = default);

    Task<Result> UpdateAsync( Guid id,  UpdateSurveyRequest request,  CancellationToken cancellationToken = default);

    Task<Result<SurveySectionDto>> AddSectionAsync( Guid surveyId, CreateSectionRequest request,  CancellationToken cancellationToken = default);

    Task<Result<QuestionDto>> AddQuestionAsync( Guid sectionId, CreateQuestionRequest request,  CancellationToken cancellationToken = default);

    Task<Result<QuestionOptionDto>> AddQuestionOptionAsync( Guid questionId,  CreateQuestionOptionRequest request,
        CancellationToken cancellationToken = default);

    Task<Result> ScheduleAsync(  Guid surveyId,  DateTime startDate,   DateTime? endDate,   CancellationToken cancellationToken = default);

    Task<Result> PublishAsync( Guid surveyId,   CancellationToken cancellationToken = default);

    Task<Result> CloseAsync(  Guid surveyId,  CancellationToken cancellationToken = default);

    Task<Result> ArchiveAsync(  Guid surveyId,    CancellationToken cancellationToken = default);


    Task<Result<SurveyResponseDto>> StartResponseAsync( Guid surveyId,  StartResponseRequest request,
    CancellationToken cancellationToken = default);

    Task<Result<ResponseAnswerDto>> AddAnswerAsync( Guid surveyId, Guid responseId, AddAnswerRequest request,
    CancellationToken cancellationToken = default);

    Task<Result<SubmitResponseDto>> SubmitResponseAsync( Guid surveyId, Guid responseId,  CancellationToken cancellationToken = default);

    Task<Result<SurveyResponseDetailsDto>> GetResponseAsync( Guid surveyId,  Guid responseId,  CancellationToken cancellationToken = default);

    Task<Result<Survey>> GetAuthorizationResourceAsync(Guid surveyId, CancellationToken cancellationToken = default);

    Task<Result<Survey>> GetSectionAuthorizationResourceAsync( Guid sectionId,  CancellationToken cancellationToken = default);

    Task<Result<Survey>> GetQuestionAuthorizationResourceAsync( Guid questionId, CancellationToken cancellationToken = default);
    
    Task<Result<Survey>> GetResponseAuthorizationResourceAsync( Guid surveyId, Guid responseId , CancellationToken cancellationToken = default);

    Task<Result<SurveyResponse>> GetResponseResourceAsync(Guid surveyId, Guid responseId,   CancellationToken cancellationToken = default);






}