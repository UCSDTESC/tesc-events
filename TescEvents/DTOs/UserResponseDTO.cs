namespace TescEvents.DTOs; 

public class UserResponseDTO {
    public Guid Id { get; set; }
    public string Pid { get; set; }
    public string Email { get; set; }
    public string First { get; set; }
    public string Last { get; set; }
    public string Phone { get; set; }
    public string ResumeUrl { get; set; }
    public string BatchId { get; set; }
}