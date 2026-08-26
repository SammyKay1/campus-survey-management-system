using CampusSurveyManagementSystem.Domain.Common;

namespace CampusSurveyManagementSystem.Domain.Surveys;

public class QuestionOption : Entity
{
    public Guid QuestionId { get; private set; }

    public string Text { get; private set; } = null!;

    public int DisplayOrder { get; private set; }

    private QuestionOption()
    {
    }

    public QuestionOption( Guid questionId, string text, int displayOrder)
    {
        Validate(questionId,text,displayOrder);

        QuestionId = questionId;
        Text = text.Trim();
        DisplayOrder = displayOrder;
    }

    

    public void Update( string text, int displayOrder)
    {
        if (string.IsNullOrWhiteSpace(text))
            throw new ArgumentException( "Option text is required.");

        Text = text.Trim();
        DisplayOrder = displayOrder;
    }

    private void Validate(Guid questionId, string text, int displayOrder)
    {
        if (questionId == Guid.Empty)
            throw new ArgumentException( "Question is required.");

        if (string.IsNullOrWhiteSpace(text))
            throw new ArgumentException( "Option text is required.");

        if (displayOrder < 0)
            throw new ArgumentException( "Display order cannot be negative.");
    }
}