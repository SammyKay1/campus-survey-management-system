
namespace CampusSurveyManagementSystem.Application.Organizations.DTOs;

public class CreateOrganizationRequest
{
    public string Name { get; init; } = string.Empty;

    public string Code { get; init; } = string.Empty;

    public string? Description { get; init; }
}