
using CampusSurveyManagementSystem.Domain.Common;

namespace CampusSurveyManagementSystem.Domain.Organizations;

public class Organization : AuditableEntity
{
    private readonly List<OrganizationMembership> _memberships = new();

    private readonly List<Guid> _surveyIds = new();

    public string Name { get; private set; } = null!;

    public string Code { get; private set; } = null!;

    public string? Description { get; private set; }

    public bool IsActive { get; private set; }

    public IReadOnlyCollection<OrganizationMembership>? Memberships => _memberships.AsReadOnly();

    private Organization()
    {
    }

    public Organization( string name, string code,  string? description = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException( "Organization name is required.");

        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException( "Organization code is required.");

        Name = name.Trim();
        Code = code.Trim().ToUpperInvariant();
        Description = description?.Trim();
        IsActive = true;
    }

    public void Update( string name,  string? description)
      {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException( "Organization name is required.");
            

        Name = name.Trim();
        Description = description?.Trim();

        MarkUpdated();
    }

    public void Deactivate()
    {
        IsActive = false;
        MarkUpdated();
    }

    public void Activate()
    {
        IsActive = true;
        MarkUpdated();
    }

}