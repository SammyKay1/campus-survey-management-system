
public static class DependencyInjection
{
    public static IServiceCollection  AddInfrastructure(this IServiceCollection services,   IConfiguration configuration)
    {
        //services.AddDbContext<ApplicationDbContext>( options => options.UseSqlite( configuration.GetConnectionString("DefaultConnection")));

        // Identity
        // Repositories
        // External integrations

        return services;
    }
}
