using TescEvents.Models;

namespace TescEvents.Services; 

public interface IEventQuestionService {

    /// <summary>
    /// Returns all of an event's application questions
    /// </summary>
    /// <param name="eventId"></param>
    /// <returns>The application questions associated with a given event</returns>
    IEnumerable<ApplicationQuestion>? GetEventApplicationQuestions(Guid eventId);

    // Submit user's application
    /// <summary>
    /// Submits a user's application
    /// </summary>
    /// <param name="eventId"></param>
    /// <returns>The application questions associated with a given event</returns>
    void SubmitApplication(Guid eventId, Guid userId, IEnumerable<ApplicationQuestion> answers);

    // Get event application question
    ApplicationQuestion? GetEventApplicationQuestion(Guid eventId, Guid questionId);

    // Set user's response to application question
    void SetResponse(Guid eventId, Guid userId, Guid questionId, string response);

    // Get user's response to application question
    string? GetResponse(Guid eventId, Guid userId, Guid questionId);

    // Update user's response to application question
    void UpdateResponse(Guid eventId, Guid userId, Guid questionId, string response);
}