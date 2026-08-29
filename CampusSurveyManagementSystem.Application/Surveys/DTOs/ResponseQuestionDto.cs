
namespace CampusSurveyManagementSystem.Application.Surveys.DTOs;

public class ResponseQuestionDto
{
    public Guid QuestionId { get; set; }

    public string Text { get; set; } = null!;

    public string Type { get; set; } = null!;

    public bool IsRequired { get; set; }

    public int DisplayOrder { get; set; }

    public IReadOnlyCollection<ResponseQuestionOptionDto> Options { get; set; }   = Array.Empty<ResponseQuestionOptionDto>();

    public ResponseAnswerDto? Answer { get; set; }
}