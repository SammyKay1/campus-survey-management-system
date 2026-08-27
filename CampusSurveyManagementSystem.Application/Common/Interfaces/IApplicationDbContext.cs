
using CampusSurveyManagementSystem.Domain.Organizations;
using CampusSurveyManagementSystem.Domain.Surveys;
using CampusSurveyManagementSystem.Domain.Responses;
using Microsoft.EntityFrameworkCore;

namespace CampusSurveyManagementSystem.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Organization> Organizations { get; }

    DbSet<OrganizationMembership> OrganizationMemberships { get; }

    DbSet<Survey> Surveys { get; }

    DbSet<SurveySection> SurveySections { get; }

    DbSet<Question> Questions { get; }

    DbSet<QuestionOption> QuestionOptions { get; }

    DbSet<SurveyResponse> SurveyResponses { get; }

    DbSet<ResponseAnswer> ResponseAnswers { get; }

    Task<int> SaveChangesAsync( CancellationToken cancellationToken = default);
}