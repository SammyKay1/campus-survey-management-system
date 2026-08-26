using CampusSurveyManagementSystem.Domain.Common;

namespace CampusSurveyManagementSystem.Domain.Organizations;

public class OrganizationMembership : Entity
{
    public Guid OrganizationId { get; private set; }

    public Guid UserId { get; private set; }

    public OrganizationRole Role { get; private set; }

    public bool IsActive { get; private set; }

    public DateTime JoinedAt { get; private set; }

    private OrganizationMembership()
    {
    }

    public OrganizationMembership(   Guid organizationId,  Guid userId,  OrganizationRole role)
    {
        if (organizationId == Guid.Empty)
            throw new ArgumentException( "Organization is required.");

        if (userId == Guid.Empty)
            throw new ArgumentException(  "User is required.");

        OrganizationId = organizationId;
        UserId = userId;
        Role = role;
        IsActive = true;
        JoinedAt = DateTime.UtcNow;
    }

    public void ChangeRole( OrganizationRole role)
    {
        Role = role;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void Activate()
    {
        IsActive = true;
    }
}