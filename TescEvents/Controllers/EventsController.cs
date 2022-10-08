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
    public IEnumerable<Event> GetEvents(string? start = "", string? end = "") {
        if (!DateTime.TryParse(start, out var startFilter)) {
            startFilter = DateTime.UnixEpoch;
        }

        if (!DateTime.TryParse(end, out var endFilter)) {
            endFilter = DateTime.Now;
        }

        return eventRepository.FindByCondition(e => e.Start >= startFilter && e.End <= endFilter);
    }

    [HttpPost(Name = nameof(CreateEvent))]
    public IActionResult CreateEvent(Event? e) {
        if (e == null) return BadRequest();
        
        // TODO: Use automapper
        eventRepository.Create(e);
        eventRepository.Save();

        return CreatedAtRoute(nameof(GetEvents), new { id = e.Id }, e);
    }
}