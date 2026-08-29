
using CampusSurveyManagementSystem.Application.Common.Interfaces;
using CampusSurveyManagementSystem.Domain;
using CampusSurveyManagementSystem.Domain.Auditing;
using CampusSurveyManagementSystem.Domain.Organizations;
using CampusSurveyManagementSystem.Domain.Responses;
using CampusSurveyManagementSystem.Domain.Surveys;
using CampusSurveyManagementSystem.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CampusSurveyManagementSystem.Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    // DbSets will go here.

    public DbSet<Organization> Organizations => Set<Organization>();

    public DbSet<OrganizationMembership> OrganizationMemberships => Set<OrganizationMembership>();

    public DbSet<Survey> Surveys => Set<Survey>();

    public DbSet<SurveySection> SurveySections => Set<SurveySection>();

    public DbSet<Question> Questions => Set<Question>();

    public DbSet<QuestionOption> QuestionOptions => Set<QuestionOption>();

    public DbSet<SurveyResponse> SurveyResponses => Set<SurveyResponse>();

    public DbSet<ResponseAnswer> ResponseAnswers => Set<ResponseAnswer>();

    public DbSet<ResponseAnswerOption> ResponseAnswerOptions => Set<ResponseAnswerOption>();

    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();




}