
using CampusSurveyManagementSystem.Domain.Surveys;

namespace CampusSurveyManagementSystem.Application.Surveys.DTOs;

public class SurveyDto
{
    public Guid Id { get; init; }

    public Guid OrganizationId { get; init; }

    public string Title { get; init; } = null!;

    public string? Description { get; init; }

    public SurveyStatus  Status { get; init; }

    public DateTime? StartDate { get; init; }

    public DateTime? EndDate { get; init; }

    public bool IsAnonymous { get; init; }

    public DateTime CreatedAt { get; init; }

    public IReadOnlyCollection<SurveySectionDto> Sections { get; init; }  = Array.Empty<SurveySectionDto>();
}