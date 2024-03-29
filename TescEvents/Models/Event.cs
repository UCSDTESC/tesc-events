using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TescEvents.Models; 

[Table("Events")]
public class Event {
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    [MaxLength(255)]
    public string Title { get; set; }

    [Column(TypeName = "text")] 
    public string Description { get; set; } = "";

    public string? Thumbnail { get; set; }
    
    public string? Cover { get; set; }

    [Required]
    public DateTime Start { get; set; }
    
    [Required]
    public DateTime End { get; set; }

    public bool Archived { get; set; } = false;

    public bool RequiresApplication {get; set;} = false;

    public DateTime OpenDate { get; set; }

    public DateTime CloseDate { get; set; }

    public bool AcceptingApplications { get; set;}

    public bool RequiresResume { get; set;}
}