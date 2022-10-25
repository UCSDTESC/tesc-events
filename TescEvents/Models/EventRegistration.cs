using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TescEvents.Models; 

[Table("EventRegistrations")]
[Index(nameof(EventId), IsUnique = true)]
public class EventRegistration {
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public Guid Id { get; set; }
    
    public Guid StudentId { get; set; }
    
    [ForeignKey(nameof(StudentId))]
    public Student Student { get; set; }
    
    public Guid EventId { get; set; }
    
    [ForeignKey(nameof(EventId))]
    public Event Event { get; set; } // Navigation property

    public string UserStatus { get; set; } = UserStatuses.PENDING;

    public bool IsResumeSanitized { get; set; } = false;
}

public class UserStatuses {
    public const string PENDING = "PENDING";
    public const string ACCEPTED = "ACCEPTED";
    public const string REJECTED = "REJECTED";

    public const string COMMITTED = "COMMITTED";
    public const string DECLINED = "DECLINED";
}