
using CampusSurveyManagementSystem.Application.Surveys.DTOs;
using CampusSurveyManagementSystem.Application.Surveys.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CampusSurveyManagementSystem.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SurveysController : ControllerBase
{
    private readonly ISurveyService _surveyService;

    public SurveysController(ISurveyService surveyService)
    {
        _surveyService = surveyService;
    }

    // ============================================================
    // GET: api/Surveys
    // ============================================================

    [HttpGet]
    public async Task<IActionResult> GetAll( [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20,
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

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id,  CancellationToken cancellationToken = default)
    {
        var result = await _surveyService.GetByIdAsync( id, cancellationToken);

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
    public async Task<IActionResult> Create( [FromBody] CreateSurveyRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _surveyService.CreateAsync( request,  cancellationToken);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return CreatedAtAction(nameof(GetById), new { id = result.Value!.Id },   result.Value);
    }


    // ============================================================
    // PUT: api/Surveys/{id}
    // ============================================================

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id,[FromBody] UpdateSurveyRequest request,  CancellationToken cancellationToken = default)
    {
        var result = await _surveyService.UpdateAsync( id, request,  cancellationToken);

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
    public async Task<IActionResult> AddSection(Guid surveyId,  [FromBody] CreateSectionRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await _surveyService.AddSectionAsync(surveyId, request,   cancellationToken);

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
    public async Task<IActionResult> AddQuestion( Guid sectionId, [FromBody] CreateQuestionRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await _surveyService.AddQuestionAsync( sectionId, request,  cancellationToken);

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
    public async Task<IActionResult> AddQuestionOption(  Guid questionId,   [FromBody] CreateQuestionOptionRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await _surveyService.AddQuestionOptionAsync( questionId, request, cancellationToken);

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
        var result = await _surveyService.ScheduleAsync(id, request.StartDate,  request.EndDate,
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
    public async Task<IActionResult> Publish(Guid id,  CancellationToken cancellationToken = default)
    {
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
        var result = await _surveyService.CloseAsync( id,  cancellationToken);

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
    public async Task<IActionResult> Archive( Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _surveyService.ArchiveAsync( id, cancellationToken);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return NoContent();
    }
}