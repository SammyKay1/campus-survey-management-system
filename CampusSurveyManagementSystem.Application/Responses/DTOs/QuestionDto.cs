
using CampusSurveyManagementSystem.Domain.Surveys;

namespace CampusSurveyManagementSystem.Application.Surveys.DTOs;

public class QuestionDto
{
    public Guid Id { get; init; }

    public Guid SurveySectionId { get; set; }

    public string Text { get; init; } = null!;

    public QuestionType Type { get; set; }

    public bool IsRequired { get; init; }

    public int DisplayOrder { get; init; }

    public IReadOnlyCollection<QuestionOptionDto> Options { get; init; } = Array.Empty<QuestionOptionDto>();
}