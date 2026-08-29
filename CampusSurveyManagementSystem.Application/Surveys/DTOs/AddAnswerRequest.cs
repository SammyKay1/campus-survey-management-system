
using CampusSurveyManagementSystem.Domain.Surveys;

namespace CampusSurveyManagementSystem.Application.Surveys.DTOs;

public class AddAnswerRequest
{
    public Guid QuestionId { get; set; }

    public string? TextValue { get; set; }

    public Guid? SelectedOptionId { get; set; }

    public List<Guid> SelectedOptionIds { get; set; } = new();
}