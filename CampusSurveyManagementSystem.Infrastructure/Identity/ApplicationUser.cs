
using Microsoft.AspNetCore.Identity;

namespace CampusSurveyManagementSystem.Infrastructure.Identity;

public class ApplicationUser : IdentityUser<Guid>
{
    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public bool IsActive { get; set; } = true;
}