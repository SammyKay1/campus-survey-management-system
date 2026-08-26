
namespace CampusSurveyManagementSystem.Application.Surveys.DTOs;

public class SurveySectionDto
{
    public Guid Id { get; init; }

    public string Title { get; init; } = null!;

    public string? Description { get; init; }

    public int DisplayOrder { get; init; }

    public IReadOnlyCollection<QuestionDto> Questions { get; init; }  = Array.Empty<QuestionDto>();
}