using CampusSurveyManagementSystem.Domain.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CampusSurveyManagementSystem.Infrastructure.Persistence.Configurations;

public class SurveyResponseConfiguration  : IEntityTypeConfiguration<SurveyResponse>
{
    public void Configure(  EntityTypeBuilder<SurveyResponse> builder)
    {
        builder.ToTable("SurveyResponses");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.SurveyId).IsRequired();

        builder.Property(x => x.RespondentUserId);

        builder.Property(x => x.Status).IsRequired().HasConversion<int>();

        builder.Property(x => x.StartedAt).IsRequired();

        builder.Property(x => x.SubmittedAt);

        builder.HasIndex(x => x.SurveyId);

        builder.HasIndex(x => x.RespondentUserId);

        builder.HasIndex(x => x.Status);

        // Survey → Responses
        builder.HasOne<Domain.Surveys.Survey>()
            .WithMany()
            .HasForeignKey(x => x.SurveyId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}