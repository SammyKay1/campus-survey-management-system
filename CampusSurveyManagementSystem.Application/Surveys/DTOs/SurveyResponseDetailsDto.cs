namespace CampusSurveyManagementSystem.Application.Surveys.DTOs;

public class SurveyResponseDetailsDto
{
    public Guid Id { get; set; }

    public Guid SurveyId { get; set; }

    public string Status { get; set; } = null!;

    public DateTime StartedAt { get; set; }

    public DateTime? SubmittedAt { get; set; }

    public IReadOnlyCollection<ResponseQuestionDto> Questions { get; set; }   = Array.Empty<ResponseQuestionDto>();
}