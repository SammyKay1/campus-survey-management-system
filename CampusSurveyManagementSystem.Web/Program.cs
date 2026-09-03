
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
using CampusSurveyManagementSystem.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using CampusSurveyManagementSystem.Application.Identity.Interfaces;
using CampusSurveyManagementSystem.Application.Common.Authorization;
using CampusSurveyManagementSystem.Application.Identity.Constants;
using Microsoft.AspNetCore.Authorization;
using CampusSurveyManagementSystem.Infrastructure.Organizations;
using CampusSurveyManagementSystem.Web.Authorization.Handlers;
using CampusSurveyManagementSystem.Web.Authorization.Requirements;
using System.Net;
using CampusSurveyManagementSystem.Web.Authorization;
using CampusSurveyManagementSystem.Domain.Organizations;
using CampusSurveyManagementSystem.Web.Services;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
/*.EnableDetailedErrors()
.EnableSensitiveDataLogging()
.LogTo( Console.WriteLine, LogLevel.Information)); */

builder.Services
    .AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
    {
        options.User.RequireUniqueEmail = true;

        options.Password.RequiredLength = 8;
        options.Password.RequireDigit = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireNonAlphanumeric = false;

        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);

        options.SignIn.RequireConfirmedEmail = false;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(
        AuthorizationPolicies.ManageSurvey,
        policy =>
        {
            policy.RequireAuthenticatedUser();

            policy.AddRequirements(
                new OrganizationPermissionRequirement( OrganizationPermission.ManageSurvey));
        });

    options.AddPolicy(
        AuthorizationPolicies.PublishSurvey,
        policy =>
        {
            policy.RequireAuthenticatedUser();

            policy.AddRequirements(
                new OrganizationPermissionRequirement(
                    OrganizationPermission.PublishSurvey));
        });

    options.AddPolicy(
    AuthorizationPolicies.ResponseOwner,  policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.AddRequirements( new ResponseOwnerRequirement());
    });

    options.AddPolicy(
    AuthorizationPolicies.ResponseAccess,
    policy =>
    {
        policy.RequireAuthenticatedUser();

        policy.AddRequirements(
            new ResponseAccessRequirement());
    });

    options.AddPolicy(
        AuthorizationPolicies.ViewSurvey,
        policy =>
        {
            policy.RequireAuthenticatedUser();

            policy.AddRequirements(
                new OrganizationPermissionRequirement(
                    OrganizationPermission.ViewSurvey));
        });

    options.AddPolicy(
        AuthorizationPolicies.ViewResponses,
        policy =>
        {
            policy.RequireAuthenticatedUser();

            policy.AddRequirements(
                new OrganizationPermissionRequirement(
                    OrganizationPermission.ViewResponses));
        });

    options.AddPolicy(
        AuthorizationPolicies.ExportResponses,
        policy =>
        {
            policy.RequireAuthenticatedUser();

            policy.AddRequirements(
                new OrganizationPermissionRequirement(
                    OrganizationPermission.ExportResponses));
        });
});


builder.Services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

builder.Services.AddScoped<IOrganizationService, OrganizationService>();
builder.Services.AddScoped<ISurveyService, SurveyService>();
builder.Services.AddScoped<IOrganizationMembershipService, OrganizationMembershipService>();

builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddScoped<IOrganizationAccessService, OrganizationAccessService>();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, ApplicationUserClaimsPrincipalFactory>();
builder.Services.AddScoped<IAuthorizationHandler, OrganizationAccessHandler>();
builder.Services.AddScoped<IAuthorizationHandler,  OrganizationResourceAuthorizationHandler>();
builder.Services.AddScoped<IAuthorizationHandler, ResponseOwnerAuthorizationHandler>();
builder.Services.AddScoped<IAuthorizationHandler, ResponseAccessAuthorizationHandler>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();


/*builder.Services.AddIdentityCore<ApplicationUser>(options =>
    {
        options.User.RequireUniqueEmail = true;

        options.Password.RequiredLength = 8;
        options.Password.RequireDigit = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireNonAlphanumeric = false;
    })
    .AddRoles<IdentityRole<Guid>>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager(); */


builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = "CSMS.Auth";

    options.LoginPath = "/login";
    options.AccessDeniedPath = "/access-denied";

    options.SlidingExpiration = true;

    options.ExpireTimeSpan = TimeSpan.FromHours(8);
});



builder.Services.AddAuthentication();

builder.Services.AddAuthorization();





// Add services to the container.
builder.Services.AddRazorComponents().AddInteractiveServerComponents();

builder.Services.AddControllers().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter());
    });

builder.Services.AddOpenApi();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

    await IdentitySeeder.SeedRolesAsync(roleManager);
}

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

app.UseAuthentication();

app.UseAuthorization();

app.UseAntiforgery();

app.MapStaticAssets();

//
app.MapControllers();

//
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();



app.Run();
