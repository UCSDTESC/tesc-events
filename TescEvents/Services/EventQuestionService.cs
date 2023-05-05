using Microsoft.EntityFrameworkCore;
using TescEvents.Entities;
using TescEvents.Models;
using TescEvents.Repositories;

namespace TescEvents.Services;

public class EventQuestionService : IEventQuestionService {

    private RepositoryContext context;
    public EventQuestionService(RepositoryContext context) {
        this.context = context;
    }

    public IEnumerable<ApplicationQuestion>? GetEventApplicationQuestions(Guid eventId) {
        throw new NotImplementedException();
    }

    public void SubmitApplication(Guid eventId, Guid userId, IEnumerable<ApplicationQuestion> answers) {
        throw new NotImplementedException();
    }

    public ApplicationQuestion? GetEventApplicationQuestion(Guid eventId, Guid questionId) {
        throw new NotImplementedException();
    }

    public void SetResponse(Guid eventId, Guid userId, Guid questionId, string response) {
        throw new NotImplementedException();
    }

    public string? GetResponse(Guid eventId, Guid userId, Guid questionId) {
        throw new NotImplementedException();
    }

    public void UpdateResponse(Guid eventId, Guid userId, Guid questionId, string response) {
        throw new NotImplementedException();
    }
    


  
}