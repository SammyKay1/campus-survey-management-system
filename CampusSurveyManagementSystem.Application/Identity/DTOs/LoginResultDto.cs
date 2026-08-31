
namespace CampusSurveyManagementSystem.Application.Identity.DTOs;

public class LoginResultDto
{
    public Guid UserId { get; set; }

    public string Email { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public bool IsAuthenticated { get; set; }
}