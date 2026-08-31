
namespace CampusSurveyManagementSystem.Application.Identity.Constants;

public static class Policies
{
    public const string RequireSuperAdmin = "RequireSuperAdmin";

    public const string RequireOrganizationAdmin = "RequireOrganizationAdmin";

    public const string RequireSurveyManager = "RequireSurveyManager";

    public const string CanManageOrganizations = "CanManageOrganizations";

    public const string CanManageUsers = "CanManageUsers";

    public const string CanManageSurveys = "CanManageSurveys";

    public const string CanViewResponses = "CanViewResponses";

    public const string CanViewAnalytics = "CanViewAnalytics";

    public const string CanExportResponses = "CanExportResponses";

    public const string CanManageSurvey = "CanManageSurvey";
}