using CampusSurveyManagementSystem.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace CampusSurveyManagementSystem.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication( this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>( options => options.UseSqlite( configuration.GetConnectionString("DefaultConnection")));

    

        return services;
    }
}
