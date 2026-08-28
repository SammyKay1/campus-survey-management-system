
using CampusSurveyManagementSystem.Application.Common.Interfaces;
using CampusSurveyManagementSystem.Application.Common.Models;
using CampusSurveyManagementSystem.Application.Surveys.DTOs;
using CampusSurveyManagementSystem.Application.Surveys.Interfaces;
using CampusSurveyManagementSystem.Domain.Surveys;
using Microsoft.EntityFrameworkCore;

namespace CampusSurveyManagementSystem.Application.Surveys.Services;

public class SurveyService : ISurveyService
{
    private readonly IApplicationDbContext _context;

    public SurveyService(IApplicationDbContext context)
    {
        _context = context;
    }

    // ============================================================
    // Get Survey
    // ============================================================

    public async Task<Result<SurveyDto>> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var survey = await _context.Surveys
            .AsNoTracking()
            .Include(x => x.Sections)
                .ThenInclude(x => x.Questions)
                    .ThenInclude(x => x.Options)
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);

        if (survey is null)
        {
            return Result<SurveyDto>.Failure(
                "Survey was not found.");
        }

        return Result<SurveyDto>.Success(
            MapToDto(survey));
    }


    // ============================================================
    // Get Surveys
    // ============================================================

    public async Task<Result<PagedResult<SurveyDto>>> GetAllAsync(
        int pageNumber = 1,
        int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        if (pageNumber < 1)
        {
            return Result<PagedResult<SurveyDto>>.Failure(
                "Page number must be greater than zero.");
        }

        if (pageSize < 1 || pageSize > 100)
        {
            return Result<PagedResult<SurveyDto>>.Failure(
                "Page size must be between 1 and 100.");
        }

        var query = _context.Surveys
            .AsNoTracking()
            .OrderBy(x => x.Title);

        var totalCount = await query.CountAsync(
            cancellationToken);

        var surveys = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var items = surveys
            .Select(MapToDto)
            .ToArray();

        var result = new PagedResult<SurveyDto>
        {
            Items = items,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };

        return Result<PagedResult<SurveyDto>>.Success(
            result);
    }


    // ============================================================
    // Create Survey
    // ============================================================

    public async Task<Result<SurveyDto>> CreateAsync(
        CreateSurveyRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request.OrganizationId == Guid.Empty)
        {
            return Result<SurveyDto>.Failure(
                "Organization is required.");
        }

        var organizationExists = await _context.Organizations
            .AnyAsync(
                x => x.Id == request.OrganizationId,
                cancellationToken);

        if (!organizationExists)
        {
            return Result<SurveyDto>.Failure(
                "Organization was not found.");
        }

        var survey = new Survey(
            request.OrganizationId,
            request.Title,
            request.Description,
            request.IsAnonymous);

        _context.Surveys.Add(survey);

        await _context.SaveChangesAsync(
            cancellationToken);

        return Result<SurveyDto>.Success(
            MapToDto(survey));
    }


    // ============================================================
    // Update Survey
    // ============================================================

    public async Task<Result> UpdateAsync(
        Guid id,
        UpdateSurveyRequest request,
        CancellationToken cancellationToken = default)
    {
        var survey = await _context.Surveys
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);

        if (survey is null)
        {
            return Result.Failure(
                "Survey was not found.");
        }

        survey.Update(
            request.Title,
            request.Description);

        await _context.SaveChangesAsync(
            cancellationToken);

        return Result.Success();
    }


    // ============================================================
    // Add Section
    // ============================================================

    public async Task<Result<SurveySectionDto>> AddSectionAsync( Guid surveyId,   CreateSectionRequest request,
        CancellationToken cancellationToken = default)
    {
        var survey = await _context.Surveys.FirstOrDefaultAsync( x => x.Id == surveyId, cancellationToken);

        if (survey is null)
        {
            return Result<SurveySectionDto>.Failure( "Survey was not found.");
        }

        var section = new SurveySection(surveyId, request.Title, request.DisplayOrder, request.Description);

        survey.AddSection(section);

        _context.SurveySections.Add(section);

        if (!string.IsNullOrWhiteSpace(request.Description))
        {
            section.Update( request.Title,  request.Description);
        }

        await _context.SaveChangesAsync( cancellationToken);

        return Result<SurveySectionDto>.Success( MapToDto(section));
    }


    // ============================================================
    // Add Question
    // ============================================================

    public async Task<Result<QuestionDto>> AddQuestionAsync( Guid sectionId,  CreateQuestionRequest request,
        CancellationToken cancellationToken = default)
    {
        var section = await _context.SurveySections.FirstOrDefaultAsync( x => x.Id == sectionId, cancellationToken);

        if (section is null)
        {
            return Result<QuestionDto>.Failure( "Survey section was not found.");
        }

        var question = new Question(sectionId, request.Text,   request.Type,  request.IsRequired,  request.DisplayOrder);

        section.AddQuestion(question);

        _context.Questions.Add(question);

        await _context.SaveChangesAsync( cancellationToken);

        return Result<QuestionDto>.Success(  MapToDto(question));
    }


    // ============================================================
    // Add Question Option
    // ============================================================

    public async Task<Result<QuestionOptionDto>> AddQuestionOptionAsync( Guid questionId, CreateQuestionOptionRequest request,
        CancellationToken cancellationToken = default)
    {
        var question = await _context.Questions.FirstOrDefaultAsync( x => x.Id == questionId,   cancellationToken);

        if (question is null)
        {
            return Result<QuestionOptionDto>.Failure( "Question was not found.");
        }

        var option = new QuestionOption(questionId, request.Text,  request.DisplayOrder);

        question.AddOption(option);

        _context.QuestionOptions.Add(option);

        await _context.SaveChangesAsync(cancellationToken);

        return Result<QuestionOptionDto>.Success(MapToDto(option));
    }


    // ============================================================
    // Schedule Survey
    // ============================================================
    //
    public async Task<Result> ScheduleAsync(  Guid surveyId,   DateTime startDate,   DateTime? endDate,
        CancellationToken cancellationToken = default)
    {
        var survey = await _context.Surveys
            .FirstOrDefaultAsync( x => x.Id == surveyId,  cancellationToken);

        if (survey is null)
        {
            return Result.Failure( "Survey was not found.");
        }

        survey.Schedule( startDate,  endDate);

        await _context.SaveChangesAsync( cancellationToken);

        return Result.Success();
    }


    // ============================================================
    // Publish Survey
    // ============================================================

    public async Task<Result> PublishAsync( Guid surveyId,   CancellationToken cancellationToken = default)
    {
        var survey = await _context.Surveys
            .Include(x => x.Sections)
                .ThenInclude(x => x.Questions)
            .FirstOrDefaultAsync( x => x.Id == surveyId, cancellationToken);

        if (survey is null)
        {
            return Result.Failure(  "Survey was not found.");
        }

        survey.Publish();

        await _context.SaveChangesAsync(
            cancellationToken);

        return Result.Success();
    }


    // ============================================================
    // Close Survey
    // ============================================================

    public async Task<Result> CloseAsync( Guid surveyId, CancellationToken cancellationToken = default)
    {
        var survey = await _context.Surveys
            .FirstOrDefaultAsync(
                x => x.Id == surveyId,
                cancellationToken);

        if (survey is null)
        {
            return Result.Failure(
                "Survey was not found.");
        }

        survey.Close();

        await _context.SaveChangesAsync(
            cancellationToken);

        return Result.Success();
    }


    // ============================================================
    // Archive Survey
    // ============================================================

    public async Task<Result> ArchiveAsync( Guid surveyId,  CancellationToken cancellationToken = default)
    {
        var survey = await _context.Surveys
            .FirstOrDefaultAsync(x => x.Id == surveyId,  cancellationToken);

        if (survey is null)
        {
            return Result.Failure( "Survey was not found.");
        }

        survey.Archive();

        await _context.SaveChangesAsync( cancellationToken);

        return Result.Success();
    }


    // ============================================================
    // Mapping
    // ============================================================

    private static SurveyDto MapToDto( Survey survey)
    {
        return new SurveyDto
        {
            Id = survey.Id,
            OrganizationId = survey.OrganizationId,
            Title = survey.Title,
            Description = survey.Description,
            Status = survey.Status,
            StartDate = survey.StartDate,
            EndDate = survey.EndDate,
            IsAnonymous = survey.IsAnonymous,
            CreatedAt = survey.CreatedAt,

            Sections = survey.Sections
                .OrderBy(x => x.DisplayOrder)
                .Select(MapToDto)
                .ToArray()
        };
    }


    private static SurveySectionDto MapToDto( SurveySection section)
    {
        return new SurveySectionDto
        {
            Id = section.Id,
            SurveyId = section.SurveyId,
            Title = section.Title,
            Description = section.Description,
            DisplayOrder = section.DisplayOrder,

            Questions = section.Questions
                .OrderBy(x => x.DisplayOrder)
                .Select(MapToDto)
                .ToArray()
        };
    }


    private static QuestionDto MapToDto( Question question)
    {
        return new QuestionDto
        {
            Id = question.Id,
            SurveySectionId = question.SurveySectionId,
            Text = question.Text,
            Type = question.Type,
            IsRequired = question.IsRequired,
            DisplayOrder = question.DisplayOrder,

            Options = question.Options
                .OrderBy(x => x.DisplayOrder)
                .Select(MapToDto)
                .ToArray()
        };
    }


    private static QuestionOptionDto MapToDto(QuestionOption option)
    {
        return new QuestionOptionDto
        {
            Id = option.Id,
            QuestionId = option.QuestionId,
            Text = option.Text,
            DisplayOrder = option.DisplayOrder
        };
    }
}