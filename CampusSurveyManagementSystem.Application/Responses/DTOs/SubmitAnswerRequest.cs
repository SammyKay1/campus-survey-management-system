
namespace CampusSurveyManagementSystem.Application.Responses.DTOs;

public class SubmitAnswerRequest
{
    public Guid QuestionId { get; init; }

    public string? TextValue { get; init; }

    public decimal? NumericValue { get; init; }

    public DateTime? DateValue { get; init; }

    public Guid? SelectedOptionId { get; init; }
}