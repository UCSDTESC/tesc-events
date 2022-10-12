using Microsoft.AspNetCore.Mvc;
using TescEvents.Models;
using TescEvents.Repositories;
using TescEvents.Utilities;

namespace TescEvents.Controllers; 

[ApiController]
[Route("/api/[controller]")]
public class EventsController : ControllerBase {
    private readonly IEventRepository eventRepository;
    private readonly IMapper mapper;

    public EventsController(IEventRepository eventRepository, IMapper mapper) {
        this.eventRepository = eventRepository;
        this.mapper = mapper;
    }
    
    [HttpGet(Name = nameof(GetEvents))]
    public IEnumerable<Event> GetEvents(string? start = "", string? end = "") {
        if (!DateTime.TryParse(start, out var startFilter)) {
            startFilter = DateTime.UnixEpoch;
        }

        if (!DateTime.TryParse(end, out var endFilter)) {
            endFilter = DateTime.Now;
        }

        return eventRepository.FindByCondition(
                                               e => e.Start >= startFilter 
                                                    && e.End <= endFilter);
    }

    [HttpPost(Name = nameof(CreateEvent))]
    public IActionResult CreateEvent(EventRequestDTO? e) {
        if (e == null) return BadRequest();

        var eventEntity = mapper.Map<Event>(e);
        
        eventRepository.Create(eventEntity);
        eventRepository.Save();

        var createdEvent = mapper.Map<EventResponseDTO>(eventEntity);

        return CreatedAtRoute(nameof(GetEvents), new { id = createdEvent.Id }, createdEvent);
    }
}