
using CampusSurveyManagementSystem.Domain.Responses;
using CampusSurveyManagementSystem.Domain.Surveys;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CampusSurveyManagementSystem.Infrastructure.Persistence.Configurations;

public class ResponseAnswerOptionConfiguration  : IEntityTypeConfiguration<ResponseAnswerOption>
{
    public void Configure( EntityTypeBuilder<ResponseAnswerOption> builder)
    {
        builder.ToTable("ResponseAnswerOptions");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ResponseAnswerId).IsRequired();

        builder.Property(x => x.QuestionOptionId).IsRequired();

        builder.HasIndex(x => new
        {
            x.ResponseAnswerId,
            x.QuestionOptionId
        }).IsUnique();

        // Answer → Selected Options
        builder.HasOne<ResponseAnswer>()
            .WithMany(x => x.SelectedOptions)
            .HasForeignKey(x => x.ResponseAnswerId)
            .OnDelete(DeleteBehavior.Cascade);

        // Selected Option → QuestionOption
        builder.HasOne<QuestionOption>()
            .WithMany()
            .HasForeignKey(x => x.QuestionOptionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}