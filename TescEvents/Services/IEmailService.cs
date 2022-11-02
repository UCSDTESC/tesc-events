namespace TescEvents.Services; 

public interface IEmailService {
    /// <summary>
    /// Sends an event registration confirmation email to a given email address
    /// </summary>
    /// <param name="email"></param>
    void SendEventRegistrationConfirmationEmail(string email);
    
    /// <summary>
    /// Sends a signup confirmation email to a given email address
    /// </summary>
    /// <param name="email"></param>
    void SendSignupConfirmationEmail(string email);
}