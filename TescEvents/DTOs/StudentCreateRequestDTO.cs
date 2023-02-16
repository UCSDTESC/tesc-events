using System.ComponentModel.DataAnnotations;

namespace TescEvents.DTOs; 

public class StudentCreateRequestDTO {
    public string Pid { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; } 
    public string Email { get; set; } 
    public string? Phone { get; set; }
}