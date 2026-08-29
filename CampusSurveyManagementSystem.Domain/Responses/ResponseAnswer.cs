using CampusSurveyManagementSystem.Domain.Common;

namespace CampusSurveyManagementSystem.Domain.Responses;

public class ResponseAnswer : Entity
{
    private readonly List<ResponseAnswerOption> _selectedOptions = new();

    public Guid SurveyResponseId { get; private set; }

    public Guid QuestionId { get; private set; }

    public string? TextValue { get; private set; }

    public decimal? NumericValue { get; private set; }

    public DateTime? DateValue { get; private set; }

    public Guid? SelectedOptionId { get; private set; }

    public IReadOnlyCollection<ResponseAnswerOption> SelectedOptions =>
        _selectedOptions.AsReadOnly();

    private ResponseAnswer()
    {
    }

    public ResponseAnswer( Guid surveyResponseId, Guid questionId,   string? textValue = null,   Guid? selectedOptionId = null)
    {
        ValidateIdentity(surveyResponseId, questionId);

        SurveyResponseId = surveyResponseId;
        QuestionId = questionId;
        TextValue = textValue?.Trim();
        SelectedOptionId = selectedOptionId;
    }

    public void SetText(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Text value is required.");

        TextValue = value.Trim();
        NumericValue = null;
        DateValue = null;
        SelectedOptionId = null;
        _selectedOptions.Clear();
    }

    public void SetNumber(decimal value)
    {
        TextValue = null;
        NumericValue = value;
        DateValue = null;
        SelectedOptionId = null;
        _selectedOptions.Clear();
    }

    public void SetDate(DateTime value)
    {
        TextValue = null;
        NumericValue = null;
        DateValue = value;
        SelectedOptionId = null;
        _selectedOptions.Clear();
    }

    public void SetOption(Guid optionId)
    {
        if (optionId == Guid.Empty)
            throw new ArgumentException("Option is required.");

        TextValue = null;
        NumericValue = null;
        DateValue = null;

        _selectedOptions.Clear();

        SelectedOptionId = optionId;
    }

    public void AddSelectedOption(ResponseAnswerOption selectedOption)
    {
        if (selectedOption is null)
            throw new ArgumentNullException(nameof(selectedOption));

        if (selectedOption.QuestionOptionId == Guid.Empty)
            throw new ArgumentException("Question option is required.");

        if (_selectedOptions.Any(  x => x.QuestionOptionId == selectedOption.QuestionOptionId))
        {
            throw new InvalidOperationException( "This option has already been selected.");
        }

        // Multiple-choice answers should not coexist with
        // text or single-choice values.
        TextValue = null;
        NumericValue = null;
        DateValue = null;
        SelectedOptionId = null;

        _selectedOptions.Add(selectedOption);
    }

    private static void ValidateIdentity( Guid surveyResponseId,   Guid questionId)
    {
        if (surveyResponseId == Guid.Empty)
            throw new ArgumentException( "Survey response is required.");

        if (questionId == Guid.Empty)
            throw new ArgumentException( "Question is required.");
    }
}