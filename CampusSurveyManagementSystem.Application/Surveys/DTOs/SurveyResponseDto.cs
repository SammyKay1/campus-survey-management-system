
using CampusSurveyManagementSystem.Domain.Responses;

namespace CampusSurveyManagementSystem.Application.Surveys.DTOs;

public class SurveyResponseDto
{
    public Guid Id { get; set; }

    public Guid SurveyId { get; set; }

    public Guid? RespondentUserId { get; set; }

    public SurveyResponseStatus Status { get; set; }

    public DateTime StartedAt { get; set; }

    public DateTime? SubmittedAt { get; set; }
}