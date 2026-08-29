
namespace CampusSurveyManagementSystem.Application.Surveys.DTOs;

public class ResponseAnswerDto
{
    public Guid Id { get; set; }

    public Guid SurveyResponseId { get; set; }

    public Guid QuestionId { get; set; }

    public string? TextValue { get; set; }

    public Guid? SelectedOptionId { get; set; }

    public IReadOnlyCollection<Guid> SelectedOptionIds { get; set; }  = Array.Empty<Guid>();
}