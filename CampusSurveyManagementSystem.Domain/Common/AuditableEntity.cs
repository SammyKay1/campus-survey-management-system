
namespace CampusSurveyManagementSystem.Domain.Common;

public abstract class AuditableEntity : Entity
{
    public DateTime CreatedAt { get; protected set; }

    public DateTime? UpdatedAt { get; protected set; }

    protected AuditableEntity()
    {
        CreatedAt = DateTime.UtcNow;
    }

    protected void MarkUpdated()
    {
        UpdatedAt = DateTime.UtcNow;
    }
}