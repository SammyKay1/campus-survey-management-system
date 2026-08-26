using CampusSurveyManagementSystem.Domain.Common;

namespace CampusSurveyManagementSystem.Domain.Auditing;

public class AuditLog : Entity
{
    
    public Guid? UserId { get; private set; }

    public Guid? OrganizationId { get; private set; }

    public string Action { get; private set; } = null!;

    public string EntityName { get; private set; } = null!;

    public Guid? EntityId { get; private set; }

    public DateTime Timestamp { get; private set; }

    public string? Details { get; private set; }

    private AuditLog()
    {
    }

    public AuditLog( string action, string entityName, Guid? entityId = null, Guid? userId = null, Guid? organizationId = null,
        string? details = null)
    {
        Id = Guid.NewGuid();
        Action = action.Trim();
        EntityName = entityName;
        EntityId = entityId;
        UserId = userId;
        OrganizationId = organizationId;
        Details = details?.Trim();
        Timestamp = DateTime.UtcNow;
    }
}