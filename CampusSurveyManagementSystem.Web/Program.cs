using CampusSurveyManagementSystem.Application;
using CampusSurveyManagementSystem.Web.Components;
using CampusSurveyManagementSystem.Application.Common.Interfaces;
using CampusSurveyManagementSystem.Application.Organizations.Interfaces;
using CampusSurveyManagementSystem.Application.Organizations.Services;
using CampusSurveyManagementSystem.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure( builder.Configuration);

builder.Services.AddScoped<IApplicationDbContext>( provider => provider.GetRequiredService<ApplicationDbContext>());

builder.Services.AddScoped<IOrganizationService, OrganizationService>();




// Add services to the container.
builder.Services.AddRazorComponents().AddInteractiveServerComponents();

builder.Services.AddControllers();

//builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

//app.MapControllers();

app.Run();
