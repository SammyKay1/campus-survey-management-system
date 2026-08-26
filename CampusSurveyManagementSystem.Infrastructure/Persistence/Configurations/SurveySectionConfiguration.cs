using CampusSurveyManagementSystem.Domain.Surveys;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CampusSurveyManagementSystem.Infrastructure.Persistence.Configurations;

public class SurveySectionConfiguration
    : IEntityTypeConfiguration<SurveySection>
{
    public void Configure(
        EntityTypeBuilder<SurveySection> builder)
    {
        builder.ToTable("SurveySections");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.SurveyId)
            .IsRequired();

        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(x => x.Description)
            .HasMaxLength(1000);

        builder.Property(x => x.DisplayOrder)
            .IsRequired();

        builder.HasIndex(x => new
        {
            x.SurveyId,
            x.DisplayOrder
        });

        // Survey → Sections
        builder.HasOne<Survey>()
            .WithMany(x => x.Sections)
            .HasForeignKey(x => x.SurveyId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}