using Microsoft.AspNetCore.Mvc;
using TescEvents.Models;
using TescEvents.Repositories;

namespace TescEvents.Controllers; 

[ApiController]
[Route("/api/[controller]")]
public class EventsController : ControllerBase {
    private readonly IEventRepository eventRepository;

    public EventsController(IEventRepository eventRepository) {
        this.eventRepository = eventRepository;
    }
    
    [HttpGet(Name = nameof(GetEvents))]
    public IActionResult GetEvents(string? start = "", string? end = "") {
        if (!DateTime.TryParse(start, out var startFilter)) {
            startFilter = DateTime.UnixEpoch;
        }

        if (!DateTime.TryParse(end, out var endFilter)) {
            endFilter = DateTime.Now;
        }

        return Ok(eventRepository.FindByCondition(e => e.Start >= startFilter.ToUniversalTime() && e.End <= endFilter
                                                      .ToUniversalTime()));
    }
}