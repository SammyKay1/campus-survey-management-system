
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CampusSurveyManagementSystem.Infrastructure.Persistence;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder =    new DbContextOptionsBuilder<ApplicationDbContext>();

        optionsBuilder.UseSqlite( "Data Source=campus-survey.db");

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}