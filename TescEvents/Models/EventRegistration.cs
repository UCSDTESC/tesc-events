using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TescEvents.Models; 

[Table("EventRegistrations")]
public class EventRegistration {
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public Guid Id { get; set; }
    
    // FK
    public Guid StudentId { get; set; }
    public virtual Student Student { get; set; }
    
    // FK
    public Guid EventId { get; set; }
    public virtual Event Event { get; set; }
    
    [Column(TypeName = "varchar(100)")]
    public UserStatus UserStatus { get; set; }
    public bool IsResumeSanitized { get; set; }
}

public enum UserStatus {
    Rejected,
    Accepted,
    Confirmed,
    Declined,
    Waitlisted
}