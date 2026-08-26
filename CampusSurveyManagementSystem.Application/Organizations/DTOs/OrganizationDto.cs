
namespace CampusSurveyManagementSystem.Application.Organizations.DTOs;

public class OrganizationDto
{
    public Guid Id { get; init; }

    public string Name { get; init; } = null!;

    public string Code { get; init; } = null!;

    public string? Description { get; init; }

    public bool IsActive { get; init; }

    public DateTime CreatedAt { get; init; }
}