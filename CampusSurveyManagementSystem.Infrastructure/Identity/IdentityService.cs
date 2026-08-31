


using CampusSurveyManagementSystem.Application.Common.Authorization;
using CampusSurveyManagementSystem.Application.Common.Models;
using CampusSurveyManagementSystem.Application.Identity.DTOs;
using CampusSurveyManagementSystem.Application.Identity.Interfaces;
using Microsoft.AspNetCore.Identity;


namespace CampusSurveyManagementSystem.Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    

    public IdentityService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<Result<UserDto>> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        var email = request.Email.Trim();

        var existingUser = await _userManager.FindByEmailAsync(email);

        if (existingUser is not null)
        {
            return Result<UserDto>.Failure("A user with this email already exists.", ErrorType.Conflict);
        }

        var user = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            UserName = email,
            Email = email,
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            var errorMessage = string.Join(" ", result.Errors.Select(x => x.Description));

            return Result<UserDto>.Failure(errorMessage, ErrorType.Validation);
        }

        //Assign roles after successful registration
        await _userManager.AddToRoleAsync(user,  Roles.Respondent);

        return Result<UserDto>.Success(new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email!,
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt
        });
    }

    public async Task<Result<LoginResultDto>> LoginAsync(
        LoginRequest request,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Email))
        {
            return Result<LoginResultDto>.Failure(
                "Email is required.",
                ErrorType.Validation);
        }

        if (string.IsNullOrWhiteSpace(request.Password))
        {
            return Result<LoginResultDto>.Failure(
                "Password is required.",
                ErrorType.Validation);
        }

        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user is null)
        {
            return Result<LoginResultDto>.Failure(
                "Invalid email or password.",
                ErrorType.Unauthorized);
        }

        if (!user.IsActive)
        {
            return Result<LoginResultDto>.Failure( "This account has been deactivated.", ErrorType.Forbidden);
        }

        var result = await _signInManager.PasswordSignInAsync(user, request.Password,  request.RememberMe,
            lockoutOnFailure: true);

        if (result.Succeeded)
        {
            return Result<LoginResultDto>.Success(new LoginResultDto
                {
                    UserId = user.Id,
                    Email = user.Email!,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    IsAuthenticated = true
                });
        }

        if (result.IsLockedOut)
        {
            return Result<LoginResultDto>.Failure("This account is temporarily locked. Please try again later.",
                ErrorType.Forbidden);
        }

        if (result.IsNotAllowed)
        {
            return Result<LoginResultDto>.Failure( "This account is not allowed to sign in.", ErrorType.Forbidden);
        }

        return Result<LoginResultDto>.Failure("Invalid email or password.", ErrorType.Unauthorized);
    }


    public async Task<Result> LogoutAsync( CancellationToken cancellationToken = default)
    {
        await _signInManager.SignOutAsync();

        return Result.Success();
    }





}
