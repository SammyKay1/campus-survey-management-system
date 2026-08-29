
using CampusSurveyManagementSystem.Application;
using CampusSurveyManagementSystem.Web.Components;
using CampusSurveyManagementSystem.Application.Common.Interfaces;
using CampusSurveyManagementSystem.Application.Organizations.Interfaces;
using CampusSurveyManagementSystem.Application.Organizations.Services;
using CampusSurveyManagementSystem.Infrastructure.Persistence;
using Scalar.AspNetCore;
using Microsoft.EntityFrameworkCore;
using CampusSurveyManagementSystem.Application.Surveys.Interfaces;
using CampusSurveyManagementSystem.Application.Surveys.Services;
using System.Text.Json.Serialization;
using CampusSurveyManagementSystem.Web.Infrastructure.ErrorHandling;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure( builder.Configuration);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
        /*.EnableDetailedErrors()
        .EnableSensitiveDataLogging()
        .LogTo( Console.WriteLine, LogLevel.Information)); */


builder.Services.AddScoped<IApplicationDbContext>( provider => provider.GetRequiredService<ApplicationDbContext>());

builder.Services.AddScoped<IOrganizationService, OrganizationService>();
builder.Services.AddScoped<ISurveyService, SurveyService>();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddProblemDetails();




// Add services to the container.
builder.Services.AddRazorComponents().AddInteractiveServerComponents();

builder.Services.AddControllers().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter());
    });

builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
   // app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.MapOpenApi(); // Exposes the OpenAPI JSON
    app.MapScalarApiReference(); // Exposes the Scalar UI
    
    //app.UseHsts();
}
//app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();

app.MapControllers();

app.MapRazorComponents<App>().AddInteractiveServerRenderMode();



app.Run();  
