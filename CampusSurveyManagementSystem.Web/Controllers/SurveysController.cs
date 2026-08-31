
using CampusSurveyManagementSystem.Application.Identity.Constants;
using CampusSurveyManagementSystem.Application.Surveys.DTOs;
using CampusSurveyManagementSystem.Application.Surveys.Interfaces;
using CampusSurveyManagementSystem.Web.Authorization;
using CampusSurveyManagementSystem.Web.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampusSurveyManagementSystem.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SurveysController : ControllerBase
{
    private readonly ISurveyService _surveyService;
    private readonly IAuthorizationService _authorizationService;

    public SurveysController(ISurveyService surveyService, IAuthorizationService authorizationService)
    {
        _surveyService = surveyService;
        _authorizationService = authorizationService;
    }

    // ============================================================
    // GET: api/Surveys
    // ============================================================

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _surveyService.GetAllAsync(pageNumber, pageSize, cancellationToken);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return Ok(result.Value);
    }


    // ============================================================
    // GET: api/Surveys/{id}
    // ============================================================

    [Authorize]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        var resourceResult = await _surveyService.GetAuthorizationResourceAsync(id, cancellationToken);

        if (!resourceResult.Succeeded)
        {
            return resourceResult.ToActionResult(this);

        }

        var authorizationResult =    await _authorizationService.AuthorizeAsync(User, resourceResult.Value!, AuthorizationPolicies.ViewSurvey);

        if (!authorizationResult.Succeeded)
        {
            return Forbid();
        }

        var result = await _surveyService.GetByIdAsync(id, cancellationToken);

        if (!result.Succeeded)
        {
            return NotFound(result.Errors);
        }

        return Ok(result.Value);
    }


    // ============================================================
    // POST: api/Surveys
    // ============================================================

    [HttpPost]
    [Authorize(Policy = Policies.CanManageSurveys)]
    public async Task<IActionResult> Create([FromBody] CreateSurveyRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _surveyService.CreateAsync(request, cancellationToken);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, result.Value);
    }


    // ============================================================
    // PUT: api/Surveys/{id}
    // ============================================================

    [HttpPut("{id:guid}")]
    [Authorize(Policy = Policies.CanManageSurveys)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateSurveyRequest request, CancellationToken cancellationToken = default)
    {
        var resourceResult = await _surveyService.GetAuthorizationResourceAsync(id);

        if (!resourceResult.Succeeded)
        {
            return resourceResult.ToActionResult(this);
        }

        var authorizationResult = await _authorizationService.AuthorizeAsync(User, resourceResult.Value, AuthorizationPolicies.ManageSurvey);

        if (!authorizationResult.Succeeded)
        {
            return Forbid();
        }

        var result = await _surveyService.UpdateAsync(id, request, cancellationToken);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return NoContent();
    }


    // ============================================================
    // POST: api/Surveys/{surveyId}/sections
    // ============================================================

    [HttpPost("{surveyId:guid}/sections")]
    public async Task<IActionResult> AddSection(Guid surveyId, [FromBody] CreateSectionRequest request,
        CancellationToken cancellationToken = default)
    {
        var resourceResult = await _surveyService.GetAuthorizationResourceAsync(surveyId, cancellationToken);

        if (!resourceResult.Succeeded)
        {
            return resourceResult.ToActionResult(this);
        }

        var authorizationResult = await _authorizationService.AuthorizeAsync(User, resourceResult.Value!,
        AuthorizationPolicies.ManageSurvey);

        if (!authorizationResult.Succeeded)
        {
            return Forbid();
        }

        var result = await _surveyService.AddSectionAsync(surveyId, request, cancellationToken);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return Ok(result.Value);
    }


    // ============================================================
    // POST: api/Surveys/sections/{sectionId}/questions
    // ============================================================

    [HttpPost("sections/{sectionId:guid}/questions")]
    public async Task<IActionResult> AddQuestion(Guid sectionId, [FromBody] CreateQuestionRequest request,
        CancellationToken cancellationToken = default)
    {

        var resourceResult = await _surveyService.GetSectionAuthorizationResourceAsync(sectionId, cancellationToken);

        if (!resourceResult.Succeeded)
        {
            return resourceResult.ToActionResult(this);
        }

        var authorizationResult = await _authorizationService.AuthorizeAsync(User, resourceResult.Value!, AuthorizationPolicies.ManageSurvey);

        if (!authorizationResult.Succeeded)
        {
            return Forbid();
        }

        var result = await _surveyService.AddQuestionAsync(sectionId, request, cancellationToken);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return Ok(result.Value);
    }


    // ============================================================
    // POST: api/Surveys/questions/{questionId}/options
    // ============================================================

    [HttpPost("questions/{questionId:guid}/options")]
    public async Task<IActionResult> AddQuestionOption(Guid questionId, [FromBody] CreateQuestionOptionRequest request,
        CancellationToken cancellationToken = default)
    {
        var resourceResult = await _surveyService.GetQuestionAuthorizationResourceAsync(questionId, cancellationToken);

        if (!resourceResult.Succeeded)
        {
            return resourceResult.ToActionResult(this);
        }

        var authorizationResult = await _authorizationService.AuthorizeAsync(User, resourceResult.Value!, AuthorizationPolicies.ManageSurvey);

        if (!authorizationResult.Succeeded)
        {
            return Forbid();
        }


        var result = await _surveyService.AddQuestionOptionAsync(questionId, request, cancellationToken);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return Ok(result.Value);
    }


    // ============================================================
    // PATCH: api/Surveys/{id}/schedule
    // ============================================================

    [HttpPatch("{id:guid}/schedule")]
    public async Task<IActionResult> Schedule(Guid id, [FromBody] ScheduleSurveyRequest request,
        CancellationToken cancellationToken = default)
    {
        var resourceResult = await _surveyService.GetAuthorizationResourceAsync(id, cancellationToken);

        if (!resourceResult.Succeeded)
        {
            return resourceResult.ToActionResult(this);

        }

        var authorizationResult =    await _authorizationService.AuthorizeAsync(User, resourceResult.Value!, AuthorizationPolicies.ManageSurvey);

        if (!authorizationResult.Succeeded)
        {
            return Forbid();
        }

        var result = await _surveyService.ScheduleAsync(id, request.StartDate, request.EndDate,
            cancellationToken);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return NoContent();
    }


    // ============================================================
    // PATCH: api/Surveys/{id}/publish
    // ============================================================

    [HttpPatch("{id:guid}/publish")]
    [Authorize(Policy = Policies.CanManageSurveys)]
    public async Task<IActionResult> Publish(Guid id, CancellationToken cancellationToken = default)
    {
        var resourceResult = await _surveyService.GetAuthorizationResourceAsync(id, cancellationToken);

        if (!resourceResult.Succeeded)
        {
            return resourceResult.ToActionResult(this);

        }

        var authorizationResult =    await _authorizationService.AuthorizeAsync(User, resourceResult.Value!, AuthorizationPolicies.PublishSurvey);

        if (!authorizationResult.Succeeded)
        {
            return Forbid();
        }

        var result = await _surveyService.PublishAsync(id, cancellationToken);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return NoContent();
    }


    // ============================================================
    // PATCH: api/Surveys/{id}/close
    // ============================================================

    [HttpPatch("{id:guid}/close")]
    public async Task<IActionResult> Close(Guid id, CancellationToken cancellationToken = default)
    {
        var resourceResult = await _surveyService.GetAuthorizationResourceAsync(id, cancellationToken);

        if (!resourceResult.Succeeded)
        {
            return resourceResult.ToActionResult(this);

        }

        var authorizationResult =    await _authorizationService.AuthorizeAsync(User, resourceResult.Value!, AuthorizationPolicies.ManageSurvey);

        if (!authorizationResult.Succeeded)
        {
            return Forbid();
        }

        var result = await _surveyService.CloseAsync(id, cancellationToken);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return NoContent();
    }


    // ============================================================
    // PATCH: api/Surveys/{id}/archive
    // ============================================================

    [HttpPatch("{id:guid}/archive")]
    public async Task<IActionResult> Archive(Guid id, CancellationToken cancellationToken = default)
    {
        var resourceResult = await _surveyService.GetAuthorizationResourceAsync(id, cancellationToken);

        if (!resourceResult.Succeeded)
        {
            return resourceResult.ToActionResult(this);

        }

        var authorizationResult =    await _authorizationService.AuthorizeAsync(User, resourceResult.Value!, AuthorizationPolicies.ManageSurvey);

        if (!authorizationResult.Succeeded)
        {
            return Forbid();
        }

        var result = await _surveyService.ArchiveAsync(id, cancellationToken);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return NoContent();
    }


    /// <summary>
    /// Survey Response Controller
    /// </summary>
    /// <param name="surveyId"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// 

    [AllowAnonymous]
    [HttpPost("{surveyId:guid}/responses")]
    public async Task<IActionResult> StartResponse(Guid surveyId, [FromBody] StartResponseRequest request,
    CancellationToken cancellationToken = default)
    {
        var result = await _surveyService.StartResponseAsync(surveyId, request, cancellationToken);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return Ok(result.Value);
    }



    [HttpPost("{surveyId:guid}/responses/{responseId:guid}/answers")]
    public async Task<IActionResult> AddAnswer(Guid surveyId, Guid responseId, [FromBody] AddAnswerRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await _surveyService.AddAnswerAsync(surveyId, responseId, request, cancellationToken);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return Ok(result.Value);
    }


    [HttpPost("{surveyId:guid}/responses/{responseId:guid}/submit")]
    public async Task<IActionResult> SubmitResponse(Guid surveyId, Guid responseId,
        CancellationToken cancellationToken = default)
    {
        var result = await _surveyService.SubmitResponseAsync(
            surveyId,
            responseId,
            cancellationToken);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return Ok(result.Value);
    }


    [HttpGet("{surveyId:guid}/responses/{responseId:guid}")]
    public async Task<IActionResult> GetResponse(Guid surveyId, Guid responseId,
    CancellationToken cancellationToken = default)
    {
        var resourceResult = await _surveyService.GetAuthorizationResourceAsync(surveyId, cancellationToken);

        if (!resourceResult.Succeeded)
        {
            return resourceResult.ToActionResult(this);

        }

        var authorizationResult =    await _authorizationService.AuthorizeAsync(User, resourceResult.Value!, AuthorizationPolicies.ViewResponses);

        if (!authorizationResult.Succeeded)
        {
            return Forbid();
        }

        var result = await _surveyService.GetResponseAsync(surveyId, responseId, cancellationToken);

        if (!result.Succeeded)
        {
            return NotFound(result.Errors);
        }

        return Ok(result.Value);
    }



}