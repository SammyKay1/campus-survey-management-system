
namespace CampusSurveyManagementSystem.Application.Surveys.DTOs;

public class ResponseQuestionOptionDto
{
    public Guid Id { get; set; }

    public string Text { get; set; } = null!;

    public int DisplayOrder { get; set; }
}