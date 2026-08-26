using CampusSurveyManagementSystem.Domain.Common;

namespace CampusSurveyManagementSystem.Domain.Surveys;

public class SurveySection : Entity
{
    private readonly List<Question> _questions = new();

    public Guid SurveyId { get; private set; }

    public string Title { get; private set; } = null!;

    public string? Description { get; private set; }

    public int DisplayOrder { get; private set; }

    public IReadOnlyCollection<Question> Questions =>   _questions.AsReadOnly();

    private SurveySection()
    {
    }

    public SurveySection( Guid surveyId,  string title, int displayOrder)
    {
        if (surveyId == Guid.Empty)
            throw new ArgumentException( "Survey is required.");

        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException( "Section title is required.");

        if (displayOrder < 0)
            throw new ArgumentException( "Display order cannot be negative.");

        SurveyId = surveyId;
        Title = title.Trim();
        DisplayOrder = displayOrder;
    }

    public void AddQuestion( Question question)
    {
        _questions.Add(question);
    }

    public void Update(string title,  string? description)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException( "Section title is required.");

        Title = title.Trim();
        Description = description?.Trim();
    }
}