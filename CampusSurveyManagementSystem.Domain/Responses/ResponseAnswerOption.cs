
using CampusSurveyManagementSystem.Domain.Common;

namespace CampusSurveyManagementSystem.Domain.Responses;

public class ResponseAnswerOption : Entity
{
    public Guid ResponseAnswerId { get; private set; }

    public Guid QuestionOptionId { get; private set; }

    private ResponseAnswerOption()
    {
    }

    public ResponseAnswerOption( Guid responseAnswerId,  Guid questionOptionId)
    {
        if (responseAnswerId == Guid.Empty)
        {
            throw new ArgumentException( "Response answer is required.");
        }

        if (questionOptionId == Guid.Empty)
        {
            throw new ArgumentException(  "Question option is required.");
        }

        ResponseAnswerId = responseAnswerId;
        QuestionOptionId = questionOptionId;
    }



    
}