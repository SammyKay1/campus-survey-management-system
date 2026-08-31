

using CampusSurveyManagementSystem.Application.Identity.DTOs;
using CampusSurveyManagementSystem.Application.Identity.Interfaces;
using CampusSurveyManagementSystem.Web.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampusSurveyManagementSystem.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IIdentityService _identityService;

    public AccountController(IIdentityService identityService)
    {
        _identityService = identityService;
    }


    [Authorize]
    [HttpGet("me")]
    public IActionResult Me()
    {
        var claims = User.Claims
            .Select(x => new
            {
                x.Type,
                x.Value
            });

        return Ok(claims);
    }

    

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _identityService.RegisterAsync(request, cancellationToken);

        return result.ToActionResult(this);
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _identityService.LoginAsync(request, cancellationToken);

        return result.ToActionResult(this);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout(CancellationToken cancellationToken = default)
    {
        var result = await _identityService.LogoutAsync(cancellationToken);

        return result.ToActionResult(this);
    }




}



