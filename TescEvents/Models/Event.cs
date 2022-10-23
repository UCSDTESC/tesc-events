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

    
    [MaxLength(255)]
    public string? Thumbnail { get; set; }
    
    [MaxLength(255)]
    public string? Cover { get; set; }

    [Required]
    public DateTime Start { get; set; }
    
    [Required]
    public DateTime End { get; set; }

    public bool Archived { get; set; } = false;
}