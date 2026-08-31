
using CampusSurveyManagementSystem.Application.Common.Interfaces;
using CampusSurveyManagementSystem.Application.Common.Models;
using CampusSurveyManagementSystem.Application.Surveys.DTOs;
using CampusSurveyManagementSystem.Application.Surveys.Interfaces;
using CampusSurveyManagementSystem.Domain.Responses;
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

    public async Task<Result<SurveyDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var survey = await _context.Surveys.AsNoTracking()
            .Include(x => x.Sections)
                .ThenInclude(x => x.Questions)
                    .ThenInclude(x => x.Options)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (survey is null)
        {
            return Result<SurveyDto>.Failure("Survey was not found.", ErrorType.NotFound);
        }

        return Result<SurveyDto>.Success(MapToDto(survey));
    }


    // ============================================================
    // Get Surveys
    // ============================================================

    public async Task<Result<PagedResult<SurveyDto>>> GetAllAsync(int pageNumber = 1, int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        if (pageNumber < 1)
        {
            return Result<PagedResult<SurveyDto>>.Failure("Page number must be greater than zero.", ErrorType.Validation);
        }

        if (pageSize < 1 || pageSize > 100)
        {
            return Result<PagedResult<SurveyDto>>.Failure("Page size must be between 1 and 100.", ErrorType.Validation);
        }

        var query = _context.Surveys.AsNoTracking().OrderBy(x => x.Title);

        var totalCount = await query.CountAsync(cancellationToken);

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

        return Result<PagedResult<SurveyDto>>.Success(result);
    }


    // ============================================================
    // Create Survey
    // ============================================================

    public async Task<Result<SurveyDto>> CreateAsync(CreateSurveyRequest request, CancellationToken cancellationToken = default)
    {
        if (request.OrganizationId == Guid.Empty)
        {
            return Result<SurveyDto>.Failure("Organization is required.", ErrorType.Validation);
        }

        var organizationExists = await _context.Organizations.AnyAsync(x => x.Id == request.OrganizationId, cancellationToken);

        if (!organizationExists)
        {
            return Result<SurveyDto>.Failure("Organization was not found.", ErrorType.NotFound);
        }


        var survey = new Survey(request.OrganizationId, request.Title, request.Description,
            request.IsAnonymous);

        _context.Surveys.Add(survey);

        await _context.SaveChangesAsync(cancellationToken);

        return Result<SurveyDto>.Success(MapToDto(survey));
    }


    // ============================================================
    // Update Survey
    // ============================================================

    public async Task<Result> UpdateAsync(Guid id, UpdateSurveyRequest request, CancellationToken cancellationToken = default)
    {
        var survey = await _context.Surveys.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (survey is null)
        {
            return Result.Failure("Survey was not found.", ErrorType.NotFound);
        }

        survey.Update(request.Title, request.Description);

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }


    // ============================================================
    // Add Section
    // ============================================================

    public async Task<Result<SurveySectionDto>> AddSectionAsync(Guid surveyId, CreateSectionRequest request,
        CancellationToken cancellationToken = default)
    {
        var survey = await _context.Surveys.FirstOrDefaultAsync(x => x.Id == surveyId, cancellationToken);

        if (survey is null)
        {
            return Result<SurveySectionDto>.Failure("Survey was not found.", ErrorType.NotFound);
        }

        var section = new SurveySection(surveyId, request.Title, request.DisplayOrder, request.Description);

        survey.AddSection(section);

        _context.SurveySections.Add(section);

        if (!string.IsNullOrWhiteSpace(request.Description))
        {
            section.Update(request.Title, request.Description);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Result<SurveySectionDto>.Success(MapToDto(section));
    }


    // ============================================================
    // Add Question
    // ============================================================

    public async Task<Result<QuestionDto>> AddQuestionAsync(Guid sectionId, CreateQuestionRequest request,
        CancellationToken cancellationToken = default)
    {
        var section = await _context.SurveySections.FirstOrDefaultAsync(x => x.Id == sectionId, cancellationToken);

        if (section is null)
        {
            return Result<QuestionDto>.Failure("Survey section was not found.", ErrorType.NotFound);
        }

        var question = new Question(sectionId, request.Text, request.Type, request.IsRequired, request.DisplayOrder);

        section.AddQuestion(question);

        _context.Questions.Add(question);

        await _context.SaveChangesAsync(cancellationToken);

        return Result<QuestionDto>.Success(MapToDto(question));
    }


    // ============================================================
    // Add Question Option
    // ============================================================

    public async Task<Result<QuestionOptionDto>> AddQuestionOptionAsync(Guid questionId, CreateQuestionOptionRequest request,
        CancellationToken cancellationToken = default)
    {
        var question = await _context.Questions.FirstOrDefaultAsync(x => x.Id == questionId, cancellationToken);

        if (question is null)
        {
            return Result<QuestionOptionDto>.Failure("Question was not found.", ErrorType.NotFound);
        }

        var option = new QuestionOption(questionId, request.Text, request.DisplayOrder);

        question.AddOption(option);

        _context.QuestionOptions.Add(option);

        await _context.SaveChangesAsync(cancellationToken);

        return Result<QuestionOptionDto>.Success(MapToDto(option));
    }


    // ============================================================
    // Schedule Survey
    // ============================================================
    //
    public async Task<Result> ScheduleAsync(Guid surveyId, DateTime startDate, DateTime? endDate,
        CancellationToken cancellationToken = default)
    {
        var survey = await _context.Surveys
            .FirstOrDefaultAsync(x => x.Id == surveyId, cancellationToken);

        if (survey is null)
        {
            return Result.Failure("Survey was not found.", ErrorType.NotFound);
        }

        survey.Schedule(startDate, endDate);

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }


    // ============================================================
    // Publish Survey
    // ============================================================

    public async Task<Result> PublishAsync(Guid surveyId, CancellationToken cancellationToken = default)
    {
        var survey = await _context.Surveys
            .Include(x => x.Sections)
                .ThenInclude(x => x.Questions)
            .FirstOrDefaultAsync(x => x.Id == surveyId, cancellationToken);

        if (survey is null)
        {
            return Result.Failure("Survey was not found.", ErrorType.NotFound);
        }


        survey.Publish();

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }


    // ============================================================
    // Close Survey
    // ============================================================

    public async Task<Result> CloseAsync(Guid surveyId, CancellationToken cancellationToken = default)
    {
        var survey = await _context.Surveys.FirstOrDefaultAsync(x => x.Id == surveyId, cancellationToken);

        if (survey is null)
        {
            return Result.Failure("Survey was not found.", ErrorType.NotFound);
        }

        survey.Close();

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }


    // ============================================================
    // Archive Survey
    // ============================================================

    public async Task<Result> ArchiveAsync(Guid surveyId, CancellationToken cancellationToken = default)
    {
        var survey = await _context.Surveys.FirstOrDefaultAsync(x => x.Id == surveyId, cancellationToken);

        if (survey is null)
        {
            return Result.Failure("Survey was not found.", ErrorType.NotFound);
        }

        survey.Archive();

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }


    // ============================================================
    // Mappings
    // ============================================================

    private static SurveyDto MapToDto(Survey survey)
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


    private static SurveySectionDto MapToDto(SurveySection section)
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


    private static QuestionDto MapToDto(Question question)
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



    public async Task<Result<SurveyResponseDto>> StartResponseAsync(Guid surveyId, StartResponseRequest request,
    CancellationToken cancellationToken = default)
    {
        var survey = await _context.Surveys.FirstOrDefaultAsync(x => x.Id == surveyId, cancellationToken);

        if (survey is null)
        {
            return Result<SurveyResponseDto>.Failure("Survey was not found.", ErrorType.NotFound);
        }

        if (survey.Status != SurveyStatus.Published)
        {
            return Result<SurveyResponseDto>.Failure("Only published surveys can accept responses.", ErrorType.Conflict);
        }

        var response = new SurveyResponse(surveyId);

        _context.SurveyResponses.Add(response);

        await _context.SaveChangesAsync(cancellationToken);

        return Result<SurveyResponseDto>.Success(MapToDto(response));
    }


    private static SurveyResponseDto MapToDto(SurveyResponse response)
    {
        return new SurveyResponseDto
        {
            Id = response.Id,
            SurveyId = response.SurveyId,
            RespondentUserId = response.RespondentUserId,
            Status = response.Status,
            StartedAt = response.StartedAt,
            SubmittedAt = response.SubmittedAt
        };
    }



    public async Task<Result<ResponseAnswerDto>> AddAnswerAsync(Guid surveyId, Guid responseId, AddAnswerRequest request,
    CancellationToken cancellationToken = default)
    {
        var survey = await _context.Surveys.AsNoTracking().FirstOrDefaultAsync(x => x.Id == surveyId, cancellationToken);

        if (survey is null)
        {
            return Result<ResponseAnswerDto>.Failure("Survey was not found.", ErrorType.NotFound);
        }

        if (survey.Status != SurveyStatus.Published)
        {
            return Result<ResponseAnswerDto>.Failure("Responses can only be added to published surveys.", ErrorType.Conflict);
        }

        var response = await _context.SurveyResponses
            .FirstOrDefaultAsync(x => x.Id == responseId && x.SurveyId == surveyId,
                cancellationToken);

        if (response is null)
        {
            return Result<ResponseAnswerDto>.Failure("Survey response was not found.", ErrorType.NotFound);
        }

        if (response.Status != SurveyResponseStatus.InProgress)
        {
            return Result<ResponseAnswerDto>.Failure("Answers cannot be added after the response has been submitted.", ErrorType.Conflict);
        }

        var question = await _context.Questions.FirstOrDefaultAsync(x => x.Id == request.QuestionId, cancellationToken);

        if (question is null)
        {
            return Result<ResponseAnswerDto>.Failure("Question was not found.", ErrorType.NotFound);
        }

        var sectionExists = await _context.SurveySections
            .AnyAsync(x => x.Id == question.SurveySectionId && x.SurveyId == surveyId, cancellationToken);

        if (!sectionExists)
        {
            return Result<ResponseAnswerDto>.Failure("Question does not belong to this survey.", ErrorType.Validation);
        }

        var existingAnswer = await _context.ResponseAnswers
            .AnyAsync(x => x.SurveyResponseId == responseId && x.QuestionId == request.QuestionId,
                cancellationToken);

        if (existingAnswer)
        {
            return Result<ResponseAnswerDto>.Failure("An answer for this question already exists.", ErrorType.Conflict);
        }

        ResponseAnswer answer;

        switch (question.Type)
        {
            case QuestionType.Text:

                if (string.IsNullOrWhiteSpace(request.TextValue))
                {
                    return Result<ResponseAnswerDto>.Failure("A text answer is required.", ErrorType.Validation);
                }

                if (request.SelectedOptionId.HasValue || request.SelectedOptionIds.Count > 0)
                {
                    return Result<ResponseAnswerDto>.Failure("Text questions cannot have selected options.", ErrorType.Validation);
                }

                answer = new ResponseAnswer(responseId, question.Id, request.TextValue);
                break;

            case QuestionType.SingleChoice:

                if (!request.SelectedOptionId.HasValue)
                {
                    return Result<ResponseAnswerDto>.Failure("A selected option is required.", ErrorType.Validation);
                }

                if (!string.IsNullOrWhiteSpace(request.TextValue) || request.SelectedOptionIds.Count > 0)
                {
                    return Result<ResponseAnswerDto>.Failure("Single-choice questions require exactly one selected option.", ErrorType.Validation);
                }

                var singleOptionExists = await _context.QuestionOptions.AnyAsync(x => x.Id == request.SelectedOptionId.Value &&
                             x.QuestionId == question.Id, cancellationToken);

                if (!singleOptionExists)
                {
                    return Result<ResponseAnswerDto>.Failure("The selected option does not belong to this question.", ErrorType.Validation);
                }

                answer = new ResponseAnswer(responseId, question.Id, selectedOptionId: request.SelectedOptionId);
                break;

            case QuestionType.MultipleChoice:

                if (request.SelectedOptionIds.Count == 0)
                {
                    return Result<ResponseAnswerDto>.Failure("At least one option must be selected.", ErrorType.Validation);
                }

                if (!string.IsNullOrWhiteSpace(request.TextValue) || request.SelectedOptionId.HasValue)
                {
                    return Result<ResponseAnswerDto>.Failure("Multiple-choice questions require selected options only.", ErrorType.Validation);
                }

                var distinctOptionIds = request.SelectedOptionIds.Distinct().ToList();

                if (distinctOptionIds.Count != request.SelectedOptionIds.Count)
                {
                    return Result<ResponseAnswerDto>.Failure("Duplicate options are not allowed.", ErrorType.Validation);
                }

                var validOptionCount = await _context.QuestionOptions
                        .CountAsync(x => x.QuestionId == question.Id && distinctOptionIds.Contains(x.Id),
                            cancellationToken);

                if (validOptionCount != distinctOptionIds.Count)
                {
                    return Result<ResponseAnswerDto>.Failure("One or more selected options do not belong to this question.", ErrorType.Validation);
                }

                answer = new ResponseAnswer(responseId, question.Id);

                foreach (var optionId in distinctOptionIds)
                {
                    var selectedOption = new ResponseAnswerOption(answer.Id, optionId);

                    answer.AddSelectedOption(selectedOption);
                }
                break;


            default:
                return Result<ResponseAnswerDto>.Failure("Unsupported question type.", ErrorType.Forbidden);
        }

        _context.ResponseAnswers.Add(answer);

        await _context.SaveChangesAsync(cancellationToken);

        return Result<ResponseAnswerDto>.Success(MapToDto(answer));
    }


    private static ResponseAnswerDto MapToDto(ResponseAnswer answer)
    {
        return new ResponseAnswerDto
        {
            Id = answer.Id,
            SurveyResponseId = answer.SurveyResponseId,
            QuestionId = answer.QuestionId,
            TextValue = answer.TextValue,
            SelectedOptionId = answer.SelectedOptionId,
            SelectedOptionIds = answer.SelectedOptions
                .Select(x => x.QuestionOptionId)
                .ToArray()
        };
    }





    public async Task<Result<SubmitResponseDto>> SubmitResponseAsync(
        Guid surveyId,
        Guid responseId,
        CancellationToken cancellationToken = default)
    {
        var survey = await _context.Surveys
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Id == surveyId,
                cancellationToken);

        if (survey is null)
        {
            return Result<SubmitResponseDto>.Failure(
                "Survey was not found.", ErrorType.NotFound);
        }

        if (survey.Status != SurveyStatus.Published)
        {
            return Result<SubmitResponseDto>.Failure(
                "Only published surveys can receive responses.", ErrorType.Conflict);
        }

        var response = await _context.SurveyResponses
            .FirstOrDefaultAsync(
                x => x.Id == responseId &&
                     x.SurveyId == surveyId,
                cancellationToken);

        if (response is null)
        {
            return Result<SubmitResponseDto>.Failure(
                "Survey response was not found.", ErrorType.NotFound);
        }

        if (response.Status != SurveyResponseStatus.InProgress)
        {
            return Result<SubmitResponseDto>.Failure(
                "This response has already been submitted.", ErrorType.Conflict);
        }

        var questions = await _context.Questions
            .Where(q =>
                _context.SurveySections.Any(
                    s => s.Id == q.SurveySectionId &&
                         s.SurveyId == surveyId))
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var answers = await _context.ResponseAnswers
            .Where(x => x.SurveyResponseId == responseId)
            .Include(x => x.SelectedOptions)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        // Validate required questions.
        foreach (var question in questions.Where(x => x.IsRequired))
        {
            var answer = answers.FirstOrDefault(
                x => x.QuestionId == question.Id);

            if (answer is null)
            {
                return Result<SubmitResponseDto>.Failure(
                    $"Required question '{question.Text}' has not been answered.", ErrorType.Validation);
            }
        }

        // Prevent duplicate answers.
        var duplicateQuestionIds = answers
            .GroupBy(x => x.QuestionId)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();

        if (duplicateQuestionIds.Count > 0)
        {
            return Result<SubmitResponseDto>.Failure(
                "A response cannot contain multiple answers for the same question.", ErrorType.Conflict);
        }

        // Validate every answer.
        foreach (var answer in answers)
        {
            var question = questions.FirstOrDefault(x => x.Id == answer.QuestionId);

            if (question is null)
            {
                return Result<SubmitResponseDto>.Failure(
                    "An answer references a question that does not belong to this survey.", ErrorType.Conflict);
            }

            switch (question.Type)
            {
                case QuestionType.Text:

                    if (string.IsNullOrWhiteSpace(answer.TextValue))
                    {
                        return Result<SubmitResponseDto>.Failure(
                            $"Question '{question.Text}' requires a text answer.", ErrorType.Validation);
                    }

                    break;


                case QuestionType.SingleChoice:

                    if (!answer.SelectedOptionId.HasValue)
                    {
                        return Result<SubmitResponseDto>.Failure(
                            $"Question '{question.Text}' requires a selected option.", ErrorType.Validation);
                    }

                    var singleOptionValid =
                        await _context.QuestionOptions.AnyAsync(x => x.Id == answer.SelectedOptionId &&
                                 x.QuestionId == question.Id, cancellationToken);

                    if (!singleOptionValid)
                    {
                        return Result<SubmitResponseDto>.Failure(
                            $"Invalid option for question '{question.Text}'.", ErrorType.Validation);
                    }

                    break;


                case QuestionType.MultipleChoice:

                    if (answer.SelectedOptions.Count == 0)
                    {
                        return Result<SubmitResponseDto>.Failure(
                            $"Question '{question.Text}' requires at least one selected option.", ErrorType.Validation);
                    }

                    var selectedOptionIds = answer.SelectedOptions
                        .Select(x => x.QuestionOptionId)
                        .Distinct()
                        .ToList();

                    var validOptionCount =
                        await _context.QuestionOptions
                            .CountAsync(
                                x => x.QuestionId == question.Id &&
                                     selectedOptionIds.Contains(x.Id),
                                cancellationToken);

                    if (validOptionCount != selectedOptionIds.Count)
                    {
                        return Result<SubmitResponseDto>.Failure(
                            $"One or more options are invalid for question '{question.Text}'.", ErrorType.Validation);
                    }

                    break;


                default:

                    return Result<SubmitResponseDto>.Failure(
                        $"Question '{question.Text}' has an unsupported question type.", ErrorType.Validation);
            }
        }

        response.Submit();

        await _context.SaveChangesAsync(cancellationToken);

        return Result<SubmitResponseDto>.Success(new SubmitResponseDto
        {
            ResponseId = response.Id,
            SurveyId = surveyId,
            Status = response.Status.ToString(),
            SubmittedAt = response.SubmittedAt
        });
    }


    /// <summary>
    /// Retrieve questions and their responses
    /// </summary>
    /// <param name="surveyId"></param>
    /// <param name="responseId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<SurveyResponseDetailsDto>> GetResponseAsync(
        Guid surveyId,
        Guid responseId,
        CancellationToken cancellationToken = default)
    {
        var survey = await _context.Surveys
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Id == surveyId,
                cancellationToken);

        if (survey is null)
        {
            return Result<SurveyResponseDetailsDto>.Failure("Survey was not found.", ErrorType.NotFound);
        }

        var response = await _context.SurveyResponses
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Id == responseId &&
                     x.SurveyId == surveyId,
                cancellationToken);

        if (response is null)
        {
            return Result<SurveyResponseDetailsDto>.Failure("Survey response was not found.", ErrorType.NotFound);
        }

        var questions = await _context.Questions
            .Where(q =>
                _context.SurveySections.Any(
                    s => s.Id == q.SurveySectionId &&
                         s.SurveyId == surveyId))
            .AsNoTracking()
            .OrderBy(q => q.SurveySectionId)
            .ThenBy(q => q.DisplayOrder)
            .ToListAsync(cancellationToken);

        var questionIds = questions
            .Select(x => x.Id)
            .ToList();

        var options = await _context.QuestionOptions
            .Where(x => questionIds.Contains(x.QuestionId))
            .AsNoTracking()
            .OrderBy(x => x.DisplayOrder)
            .ToListAsync(cancellationToken);

        var answers = await _context.ResponseAnswers
            .Where(x =>
                x.SurveyResponseId == responseId &&
                questionIds.Contains(x.QuestionId))
            .Include(x => x.SelectedOptions)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var questionDtos = questions.Select(question =>
            {
                var answer = answers.FirstOrDefault(x => x.QuestionId == question.Id);

                var questionOptions = options
                    .Where(x => x.QuestionId == question.Id)
                    .Select(x => new ResponseQuestionOptionDto
                    {
                        Id = x.Id,
                        Text = x.Text,
                        DisplayOrder = x.DisplayOrder
                    })
                    .ToArray();

                return new ResponseQuestionDto
                {
                    QuestionId = question.Id,
                    Text = question.Text,
                    Type = question.Type.ToString(),
                    IsRequired = question.IsRequired,
                    DisplayOrder = question.DisplayOrder,
                    Options = questionOptions,
                    Answer = answer is null ? null : MapToDto(answer)
                };
            })
            .ToArray();

        var result = new SurveyResponseDetailsDto
        {
            Id = response.Id,
            SurveyId = response.SurveyId,
            Status = response.Status.ToString(),
            StartedAt = response.StartedAt,
            SubmittedAt = response.SubmittedAt,
            Questions = questionDtos
        };

        return Result<SurveyResponseDetailsDto>.Success(result);
    }



    /// <summary>
    /// Retrieve resource for Resource based authorization
    /// </summary>
    /// <param name="surveyId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<Survey>> GetAuthorizationResourceAsync(Guid surveyId, CancellationToken cancellationToken = default)
    {
        var survey = await _context.Surveys.FirstOrDefaultAsync(x => x.Id == surveyId, cancellationToken);

        if (survey is null)
        {
            return Result<Survey>.Failure("Survey was not found.", ErrorType.NotFound);
        }

        return Result<Survey>.Success(survey);
    }



    public async Task<Result<Survey>> GetSectionAuthorizationResourceAsync(Guid sectionId,
    CancellationToken cancellationToken = default)
    {
        var section = await _context.SurveySections.AsNoTracking()
                        .FirstOrDefaultAsync(x => x.Id == sectionId, cancellationToken);

        if (section is null)
        {
            return Result<Survey>.Failure("Survey section was not found.", ErrorType.NotFound);
        }

        var survey = await _context.Surveys.FirstOrDefaultAsync(x => x.Id == section.SurveyId, cancellationToken);

        if (survey is null)
        {
            return Result<Survey>.Failure("Survey was not found.", ErrorType.NotFound);
        }

        return Result<Survey>.Success(survey);
    }


    public async Task<Result<Survey>> GetQuestionAuthorizationResourceAsync( Guid questionId,  CancellationToken cancellationToken = default)
    {
        var question = await _context.Questions
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Id == questionId,
                cancellationToken);

        if (question is null)
        {
            return Result<Survey>.Failure("Question was not found.", ErrorType.NotFound);
        }

        var section = await _context.SurveySections
            .AsNoTracking()
            .FirstOrDefaultAsync(  x => x.Id == question.SurveySectionId, cancellationToken);

        if (section is null)
        {
            return Result<Survey>.Failure( "Survey section was not found.", ErrorType.NotFound);
        }

        var survey = await _context.Surveys.FirstOrDefaultAsync( x => x.Id == section.SurveyId,  cancellationToken);

        if (survey is null)
        {
            return Result<Survey>.Failure("Survey was not found.",  ErrorType.NotFound);
        }

        return Result<Survey>.Success(survey);
    }


}