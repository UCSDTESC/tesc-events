using TescEvents.Models;

namespace TescEvents.Services; 

public class EmailService : IEmailService {
    public void SendPasswordResetEmail(string email, string code) {
        throw new NotImplementedException();
    }

    public void SendReservationConfirmationEmail(string email, Batch batch) {
        throw new NotImplementedException();
    }

    public void SendReservationUpdateEmail(string email, Batch batch) {
        throw new NotImplementedException();
    }

    public void SendReservationCancellationEmail(string email, Batch batch) {
        throw new NotImplementedException();
    }
}