using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TescEvents.Models; 

[Table("Students")]
public class Student {
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public Guid Id { get; set; }
    
    [MaxLength(255)]
    public string? Pid { get; set; }
    
    [MaxLength(255)]
    public string FirstName { get; set; }
    
    [MaxLength(255)]
    public string LastName { get; set; }
    
    [MaxLength(255)]
    public string Email { get; set; }
    
    public string PasswordHash { get; set; }
    
    public string Salt { get; set; }
    
    [MaxLength(255)]
    public string? Phone { get; set; }

    public float? Gpa { get; set; }
    
    [MaxLength(255)]
    public string? Gender { get; set; }
    
    [MaxLength(255)]
    public string? Pronouns { get; set; }
    
    [MaxLength(255)]
    public string? Ethnicity { get; set; }
    
    [MaxLength(255)]
    public string? Major { get; set; }
    
    public string? ResumeUrl { get; set; }
    
    public string? ResetToken { get; set; }
}