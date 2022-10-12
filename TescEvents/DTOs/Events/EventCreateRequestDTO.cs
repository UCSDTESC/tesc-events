namespace TescEvents.DTOs.Events; 

public class EventCreateRequestDTO {
    public string Title { get; set; }
    public string Description { get; set; }
    public IFormFile? Thumbnail { get; set; }
    public IFormFile? Cover { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
}