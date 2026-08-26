using CampusSurveyManagementSystem.Domain.Surveys;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CampusSurveyManagementSystem.Infrastructure.Persistence.Configurations;

public class QuestionOptionConfiguration   : IEntityTypeConfiguration<QuestionOption>
{
    public void Configure( EntityTypeBuilder<QuestionOption> builder)
    {
        builder.ToTable("QuestionOptions");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.QuestionId)
            .IsRequired();

        builder.Property(x => x.Text)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.DisplayOrder)
            .IsRequired();

        builder.HasIndex(x => new
        {
            x.QuestionId,
            x.DisplayOrder
        });

        // Question → Options
        builder.HasOne<Question>()
            .WithMany(x => x.Options)
            .HasForeignKey(x => x.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}