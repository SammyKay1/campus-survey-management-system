
namespace CampusSurveyManagementSystem.Application.Surveys.DTOs;

public class UpdateSurveyRequest
{
    public string Title { get; set; } = null!;

    public string? Description { get; set; }
}