
namespace CampusSurveyManagementSystem.Application.Surveys.DTOs;

public class QuestionOptionDto
{
    public Guid Id { get; init; }

    public string Text { get; init; } = null!;

    public int DisplayOrder { get; init; }
}