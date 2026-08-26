
namespace CampusSurveyManagementSystem.DTOs.Surveys;

public class SurveyListDto
{
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public string Status { get; set; } = null!;

    public string ResponseMode { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? PublishedAt { get; set; }

    public int ResponseCount { get; set; }
}
