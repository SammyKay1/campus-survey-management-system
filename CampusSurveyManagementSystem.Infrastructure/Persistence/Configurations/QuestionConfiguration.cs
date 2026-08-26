using CampusSurveyManagementSystem.Domain.Surveys;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CampusSurveyManagementSystem.Infrastructure.Persistence.Configurations;

public class QuestionConfiguration : IEntityTypeConfiguration<Question>
{
    public void Configure( EntityTypeBuilder<Question> builder)
    {
        builder.ToTable("Questions");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.SurveySectionId)
            .IsRequired();

        builder.Property(x => x.Text)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(x => x.Type)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(x => x.IsRequired)
            .IsRequired();

        builder.Property(x => x.DisplayOrder)
            .IsRequired();

        builder.HasIndex(x => new
        {
            x.SurveySectionId,
            x.DisplayOrder
        });

        // Section → Questions
        builder.HasOne<SurveySection>()
            .WithMany(x => x.Questions)
            .HasForeignKey(x => x.SurveySectionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}