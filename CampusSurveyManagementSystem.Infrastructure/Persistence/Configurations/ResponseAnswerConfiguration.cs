using CampusSurveyManagementSystem.Domain.Responses;
using CampusSurveyManagementSystem.Domain.Surveys;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CampusSurveyManagementSystem.Infrastructure.Persistence.Configurations;

public class ResponseAnswerConfiguration  : IEntityTypeConfiguration<ResponseAnswer>
{
    public void Configure(  EntityTypeBuilder<ResponseAnswer> builder)
    {
        builder.ToTable("ResponseAnswers");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.SurveyResponseId)
            .IsRequired();

        builder.Property(x => x.QuestionId)
            .IsRequired();

        builder.Property(x => x.TextValue)
            .HasMaxLength(4000);

        builder.Property(x => x.NumericValue)
            .HasPrecision(18, 4);

        builder.Property(x => x.DateValue);

        builder.Property(x => x.SelectedOptionId);

        // A response should answer a question once.
        builder.HasIndex(x => new
        {
            x.SurveyResponseId,
            x.QuestionId
        })
        .IsUnique();

        // Response → Answers
        builder.HasOne<SurveyResponse>()
            .WithMany(x => x.Answers)
            .HasForeignKey(x => x.SurveyResponseId)
            .OnDelete(DeleteBehavior.Cascade);

        // Question relationship
        builder.HasOne<Question>()
            .WithMany()
            .HasForeignKey(x => x.QuestionId)
            .OnDelete(DeleteBehavior.Restrict);

        // Selected option relationship
        builder.HasOne<QuestionOption>()
            .WithMany()
            .HasForeignKey(x => x.SelectedOptionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}