using CampusSurveyManagementSystem.Domain.Common;

namespace CampusSurveyManagementSystem.Domain.Surveys;

public class Question : Entity
{
    private readonly List<QuestionOption> _options = new();

    public Guid SurveySectionId { get; private set; }

    public string Text { get; private set; } = null!;

    public QuestionType Type { get; private set; }

    public bool IsRequired { get; private set; }

    public int DisplayOrder { get; private set; }

    public IReadOnlyCollection<QuestionOption> Options =>
        _options.AsReadOnly();

    private Question()
    {
    }

    public Question( Guid surveySectionId, string text, QuestionType type, bool isRequired, int displayOrder)
    {
        if (surveySectionId == Guid.Empty)
            throw new ArgumentException("Survey section is required.");

        if (string.IsNullOrWhiteSpace(text))
            throw new ArgumentException( "Question text is required.");

        if (displayOrder < 0)
            throw new ArgumentException("Display order cannot be negative.");

        SurveySectionId = surveySectionId;
        Text = text.Trim();
        Type = type;
        IsRequired = isRequired;
        DisplayOrder = displayOrder;
    }

    public void AddOption( QuestionOption option)
    {
        if (Type is not ( QuestionType.SingleChoice or  QuestionType.MultipleChoice))
        {
            throw new InvalidOperationException( "Options are only valid for choice questions.");
        }

        _options.Add(option);
    }

    public void Update( string text, bool isRequired)
    {
        if (string.IsNullOrWhiteSpace(text))
            throw new ArgumentException( "Question text is required.");

        Text = text.Trim();
        IsRequired = isRequired;
    }
}