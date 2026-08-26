
namespace CampusSurveyManagementSystem.Application.Surveys.DTOs;

public class CreateSurveyRequest
{
    public Guid OrganizationId { get; init; }

    public string Title { get; init; } = string.Empty;

    public string? Description { get; init; }

    public bool IsAnonymous { get; init; }

    public DateTime? StartDate { get; init; }

    public DateTime? EndDate { get; init; }
}