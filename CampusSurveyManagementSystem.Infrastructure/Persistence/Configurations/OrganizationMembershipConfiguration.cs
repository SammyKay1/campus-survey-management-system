using CampusSurveyManagementSystem.Domain.Organizations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CampusSurveyManagementSystem.Infrastructure.Persistence.Configurations;

public class OrganizationMembershipConfiguration  : IEntityTypeConfiguration<OrganizationMembership>
{
    public void Configure(EntityTypeBuilder<OrganizationMembership> builder)
    {
        builder.ToTable("OrganizationMemberships");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.OrganizationId)
            .IsRequired();

        builder.Property(x => x.UserId)
            .IsRequired();

        builder.Property(x => x.Role)
            .IsRequired();

        builder.Property(x => x.IsActive)
            .IsRequired();

        builder.Property(x => x.JoinedAt)
            .IsRequired();

        // A user can belong to an organization only once.
        builder.HasIndex(x => new
        {
            x.OrganizationId,
            x.UserId
        })
        .IsUnique();

        // Organization → Memberships
        builder.HasOne<Organization>()
            .WithMany(x => x.Memberships)
            .HasForeignKey(x => x.OrganizationId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}