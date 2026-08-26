
namespace CampusSurveyManagementSystem.Application.Common.Interfaces;

public interface IOrganizationContext
{
    Guid? OrganizationId { get; }

    bool HasOrganization { get; }
}