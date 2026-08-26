using CampusSurveyManagementSystem.Domain.Organizations;
using CampusSurveyManagementSystem.Domain.Surveys;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CampusSurveyManagementSystem.Infrastructure.Persistence.Configurations;

public class SurveyConfiguration
    : IEntityTypeConfiguration<Survey>
{
    public void Configure(
        EntityTypeBuilder<Survey> builder)
    {
        builder.ToTable("Surveys");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.OrganizationId)
            .IsRequired();

        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(x => x.Description)
            .HasMaxLength(2000);

        builder.Property(x => x.Status)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(x => x.StartDate);

        builder.Property(x => x.EndDate);

        builder.Property(x => x.IsAnonymous)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.UpdatedAt);

        builder.HasIndex(x => x.OrganizationId);

        builder.HasIndex(x => x.Status);

        // Organization → Survey
        builder.HasOne<Organization>()
            .WithMany()
            .HasForeignKey(x => x.OrganizationId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}