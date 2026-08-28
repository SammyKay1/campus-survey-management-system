
using CampusSurveyManagementSystem.Domain.Surveys;

namespace CampusSurveyManagementSystem.Application.Surveys.DTOs;

public class CreateQuestionRequest
{
    public string Text { get; set; } = null!;

    public QuestionType Type { get; set; }

    public bool IsRequired { get; set; }

    public int DisplayOrder { get; set; }
}