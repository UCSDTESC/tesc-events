namespace TescEvents.DTOs; 

public class ResetPasswordRequestDTO { 
    public string Email { get; set; }
    public string RecoveryToken { get; set; }
    public string NewPassword { get; set; }
    public string ConfirmPassword { get; set; }
    
}