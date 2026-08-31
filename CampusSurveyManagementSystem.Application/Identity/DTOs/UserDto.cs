
namespace CampusSurveyManagementSystem.Application.Identity.DTOs;

public class UserDto
{
    public Guid Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }
}

