
using CampusSurveyManagementSystem.Domain.Common;

namespace CampusSurveyManagementSystem.Domain.Responses;

public class SurveyResponse : Entity
{
    private readonly List<ResponseAnswer> _answers = new();

    public Guid SurveyId { get; private set; }

    public Guid? RespondentUserId { get; private set; }

    public SurveyResponseStatus Status { get; private set; }

    public DateTime StartedAt { get; private set; }

    public DateTime? SubmittedAt { get; private set; }

    public IReadOnlyCollection<ResponseAnswer> Answers =>   _answers.AsReadOnly();

    private SurveyResponse()
    {
    }

    public SurveyResponse( Guid surveyId,  Guid? respondentUserId = null)
    {
        if (surveyId == Guid.Empty)
            throw new ArgumentException(  "Survey is required.");

        SurveyId = surveyId;

        RespondentUserId = respondentUserId;

        Status = SurveyResponseStatus.InProgress;

        StartedAt = DateTime.UtcNow;
    }

    public void AddAnswer(ResponseAnswer answer)
    {
        if (Status != SurveyResponseStatus.InProgress)
        {
            throw new InvalidOperationException( "Answers cannot be added after submission.");
        }

        if (answer is null)
        {
            throw new ArgumentNullException( nameof(answer));
        }

        if (_answers.Any(  x => x.QuestionId == answer.QuestionId))
        {
            throw new InvalidOperationException( "An answer for this question already exists.");
        }

        _answers.Add(answer);
    }

    public void Submit()
    {
        if (Status != SurveyResponseStatus.InProgress)
        {
            throw new InvalidOperationException( "Response has already been submitted.");
        }

        Status = SurveyResponseStatus.Submitted;

        SubmittedAt = DateTime.UtcNow;
    }
}


/* using CampusSurveyManagementSystem.Domain.Common;

namespace CampusSurveyManagementSystem.Domain.Responses;

public class SurveyResponse : Entity
{
    private readonly List<ResponseAnswer> _answers = new();

    public Guid SurveyId { get; private set; }

    public Guid? UserId { get; private set; }

    public DateTime SubmittedAt { get; private set; }

    public string? SessionIdentifier { get; private set; }

    public bool IsComplete { get; private set; }

    public IReadOnlyCollection<ResponseAnswer> Answers =>   _answers.AsReadOnly();

    private SurveyResponse()
    {
    }

    public SurveyResponse( Guid surveyId, Guid? userId = null, string? sessionIdentifier = null)
    {
        if (surveyId == Guid.Empty)
            throw new ArgumentException( "Survey is required.");

        SurveyId = surveyId;
        UserId = userId;
        SessionIdentifier = sessionIdentifier;
        IsComplete = false;
    }

    public void AddAnswer( ResponseAnswer answer)
    {
        if (IsComplete)
            throw new InvalidOperationException( "A completed response cannot be modified.");

        _answers.Add(answer);
    }

    public void Complete()
    {
        IsComplete = true;
        SubmittedAt = DateTime.UtcNow;
    }
} */