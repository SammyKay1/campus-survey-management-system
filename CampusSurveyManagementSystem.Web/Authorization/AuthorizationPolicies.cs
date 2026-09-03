
namespace CampusSurveyManagementSystem.Web.Authorization;

public static class AuthorizationPolicies
{
    public const string ManageSurvey = "ManageSurvey";

    public const string PublishSurvey = "PublishSurvey";

    public const string ViewSurvey = "ViewSurvey";

    public const string ViewResponses = "ViewResponses";

    public const string ExportResponses = "ExportResponses";

    public const string ResponseOwner = "ResponseOwner";

    public const string ResponseAccess = "ResponseAccess";
}