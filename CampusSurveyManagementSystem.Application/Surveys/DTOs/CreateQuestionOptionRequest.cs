
namespace CampusSurveyManagementSystem.Application.Surveys.DTOs;

public class CreateQuestionOptionRequest
{
    public string Text { get; set; } = null!;

    public int DisplayOrder { get; set; }
}
