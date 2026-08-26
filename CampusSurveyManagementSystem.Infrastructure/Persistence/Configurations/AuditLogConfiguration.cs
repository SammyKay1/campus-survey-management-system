using CampusSurveyManagementSystem.Domain.Auditing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CampusSurveyManagementSystem.Infrastructure.Persistence.Configurations;

public class AuditLogConfiguration  : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.ToTable("AuditLogs");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.UserId);

        builder.Property(x => x.OrganizationId);

        builder.Property(x => x.Action)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.EntityName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.EntityId);

        builder.Property(x => x.Details)
            .HasMaxLength(4000);

        builder.Property(x => x.Timestamp)
            .IsRequired();

        builder.HasIndex(x => x.Timestamp);

        builder.HasIndex(x => x.UserId);

        builder.HasIndex(x => x.OrganizationId);

        builder.HasIndex(x => new
        {
            x.EntityName,
            x.EntityId
        });
    }
}