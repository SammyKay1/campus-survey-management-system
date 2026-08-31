using CampusSurveyManagementSystem.Domain.Common;

namespace CampusSurveyManagementSystem.Domain.Surveys;

public class Survey : AuditableEntity, IOrganizationResource
{
    private readonly List<SurveySection> _sections = new();

    public Guid OrganizationId { get; private set; }

    public string Title { get; private set; } = null!;

    public string? Description { get; private set; }

    public SurveyStatus Status { get; private set; }

    public DateTime? StartDate { get; private set; }

    public DateTime? EndDate { get; private set; }

    public bool IsAnonymous { get; private set; }

    public IReadOnlyCollection<SurveySection> Sections => _sections.AsReadOnly();

    private Survey()
    {
    }

    public Survey(Guid organizationId, string title, string? description = null, bool isAnonymous = false)
    {
        Validate(organizationId, title);

        OrganizationId = organizationId;
        Title = title.Trim();
        Description = description?.Trim();

        Status = SurveyStatus.Draft;

        IsAnonymous = isAnonymous;
    }

    public void Update(string title, string? description)
    {
        Validate(Status, title);

        Title = title.Trim();
        Description = description?.Trim();

        MarkUpdated();
    }

    public void Publish()
    {
        if (!_sections.Any())
            throw new DomainException("A survey must contain at least one section.");

        if (_sections.All(s => !s.Questions.Any()))
            throw new DomainException("A survey must contain at least one question.");

        if (Status != SurveyStatus.Draft)
        {
            throw new DomainException( "Only draft surveys can be published.");
        }

        Status = SurveyStatus.Published;

        MarkUpdated();
    }

    public void Close()
    {
        if (Status != SurveyStatus.Published)
            throw new DomainException("Only published surveys can be closed.");

        Status = SurveyStatus.Closed;

        MarkUpdated();
    }

    public void Archive()
    {
        Status = SurveyStatus.Archived;

        MarkUpdated();
    }

    public void Schedule(DateTime startDate, DateTime? endDate)
    {
        if (endDate.HasValue && endDate.Value <= startDate)
        {
            throw new DomainException("End date must be after start date.");
        }

        StartDate = startDate;
        EndDate = endDate;

        MarkUpdated();
    }

    public void AddSection(SurveySection section)
    {
        if (Status != SurveyStatus.Draft)
            throw new DomainException("Sections cannot be added after publication.");

        _sections.Add(section);

        MarkUpdated();
    }

    private static void Validate(Guid organizationId, string title)
    {
        if (organizationId == Guid.Empty)
            throw new ArgumentException("Organization is required.");

        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Survey title is required.");
    }

    private static void Validate(SurveyStatus status, string title)
    {
        if (status != SurveyStatus.Draft)
            throw new DomainException("Only draft surveys can be edited.");

        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Survey title is required.");
    }
}