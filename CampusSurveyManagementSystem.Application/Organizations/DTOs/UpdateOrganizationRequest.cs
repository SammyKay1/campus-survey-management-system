
namespace CampusSurveyManagementSystem.Application.Organizations.DTOs;

public class UpdateOrganizationRequest
{
    public string Name { get; init; } = string.Empty;

    public string? Description { get; init; }
}