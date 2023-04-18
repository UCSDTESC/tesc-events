namespace TescEvents.DTOs; 

public class StudentResponseDTO {
    public Guid Id { get; set; }
    public string Pid { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Phone { get; set; }
    public string ResumeUrl { get; set; }
    
    public float Gpa { get; set; }
    public string Gender { get; set; }
    public string Pronouns { get; set; }
    public string Ethnicity { get; set; }
    public string Major { get; set; }
}