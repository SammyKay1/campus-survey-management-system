using CampusSurveyManagementSystem.Domain.Common;

namespace CampusSurveyManagementSystem.Domain.Responses;

public class ResponseAnswer : Entity
{
    public Guid SurveyResponseId { get; private set; }

    public Guid QuestionId { get; private set; }

    public string? TextValue { get; private set; }

    public decimal? NumericValue { get; private set; }

    public DateTime? DateValue { get; private set; }

    public Guid? SelectedOptionId { get; private set; }

    private ResponseAnswer()
    {
    }

    public ResponseAnswer( Guid surveyResponseId, Guid questionId)
    {
        if (surveyResponseId == Guid.Empty)
            throw new ArgumentException( "Survey response is required.");

        if (questionId == Guid.Empty)
            throw new ArgumentException( "Question is required.");

        SurveyResponseId = surveyResponseId;
        QuestionId = questionId;
    }

    public void SetText( string? value)
    {
        TextValue = value;
        NumericValue = null;
        DateValue = null;
        SelectedOptionId = null;
    }

    public void SetNumber( decimal value)
    {
        TextValue = null;
        NumericValue = value;
        DateValue = null;
        SelectedOptionId = null;
    }

    public void SetDate( DateTime value)
    {
        TextValue = null;
        NumericValue = null;
        DateValue = value;
        SelectedOptionId = null;
    }

    public void SetOption(  Guid optionId)
    {
        if (optionId == Guid.Empty)
            throw new ArgumentException( "Option is required.");

        TextValue = null;
        NumericValue = null;
        DateValue = null;
        SelectedOptionId = optionId;
    }
}