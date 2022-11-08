namespace TescEvents.Services; 

public interface IEmailService {
    void SendPasswordResetEmail(string email, string code);
}