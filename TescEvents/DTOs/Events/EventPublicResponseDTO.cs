namespace TescEvents.DTOs.Events;

public class EventPublicResponseDTO {
    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public IFormFile? Thumbnail { get; set; }
    public IFormFile? Cover { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
}