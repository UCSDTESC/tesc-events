using TescEvents.Models;

namespace TescEvents.Services; 

public interface IEmailService {
    /// <summary>
    /// Sends an email to a given email address containing the recovery token used to reset the account
    /// </summary>
    /// <param name="email"></param>
    /// <param name="code"></param>
    void SendPasswordResetEmail(string email, string code);

    /// <summary>
    /// Sends an email to a given email address
    /// </summary>
    /// <param name="email"></param>
    /// <param name="batch"></param>
    void SendReservationConfirmationEmail(string email);

    /// <summary>
    /// Sends an email to a given email address, confirming the user has successfully updated their reservation
    /// </summary>
    /// <param name="email"></param>
    /// <param name="batch"></param>
    void SendReservationUpdateEmail(string email);

    /// <summary>
    /// Sends an email to a given email address, confirming the user has successfully cancelled their reservation
    /// </summary>
    /// <param name="email"></param>
    /// <param name="batch"></param>
    void SendReservationCancellationEmail(string email);
}