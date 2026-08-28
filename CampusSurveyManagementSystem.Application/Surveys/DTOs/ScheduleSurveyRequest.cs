
namespace CampusSurveyManagementSystem.Application.Surveys.DTOs;

public class ScheduleSurveyRequest
{
    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }
}