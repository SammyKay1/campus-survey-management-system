
namespace CampusSurveyManagementSystem.Application.Surveys.DTOs;

public class SubmitResponseDto
{
    public Guid ResponseId { get; set; }

    public Guid SurveyId { get; set; }

    public string Status { get; set; } = null!;

    public DateTime? SubmittedAt { get; set; }
}