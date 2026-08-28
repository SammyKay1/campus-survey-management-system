
namespace CampusSurveyManagementSystem.Application.Surveys.DTOs;

public class CreateSectionRequest
{
    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public int DisplayOrder { get; set; }
}