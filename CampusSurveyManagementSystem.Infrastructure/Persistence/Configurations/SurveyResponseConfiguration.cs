using CampusSurveyManagementSystem.Domain.Responses;
using CampusSurveyManagementSystem.Domain.Surveys;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CampusSurveyManagementSystem.Infrastructure.Persistence.Configurations;

public class SurveyResponseConfiguration   : IEntityTypeConfiguration<SurveyResponse>
{
    public void Configure( EntityTypeBuilder<SurveyResponse> builder)
    {
        builder.ToTable("SurveyResponses");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.SurveyId)
            .IsRequired();

        builder.Property(x => x.UserId);

        builder.Property(x => x.SubmittedAt);

        builder.Property(x => x.SessionIdentifier)
            .HasMaxLength(200);

        builder.Property(x => x.IsComplete)
            .IsRequired();

        builder.HasIndex(x => x.SurveyId);

        builder.HasIndex(x => x.UserId);

        builder.HasIndex(x => x.SessionIdentifier);

        // Survey → Responses
        builder.HasOne<Survey>()
            .WithMany()
            .HasForeignKey(x => x.SurveyId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}