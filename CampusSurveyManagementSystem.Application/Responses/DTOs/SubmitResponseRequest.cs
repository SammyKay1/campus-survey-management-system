
namespace CampusSurveyManagementSystem.Application.Responses.DTOs;

public class SubmitResponseRequest
{
    public Guid SurveyId { get; init; }

    public Guid? UserId { get; init; }

    public string? SessionIdentifier { get; init; }

    public IReadOnlyCollection<SubmitAnswerRequest> Answers { get; init; }
        = Array.Empty<SubmitAnswerRequest>();
}