using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TescEvents.DTOs;
using TescEvents.DTOs.Events;
using TescEvents.Models;
using TescEvents.Repositories;

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
    public IActionResult GetEvents(string? start = "", string? end = "") {
        if (!DateTime.TryParse(start, out var startFilter)) {
            startFilter = DateTime.Now;
        }

        if (!DateTime.TryParse(end, out var endFilter)) {
            endFilter = DateTime.MinValue;
        }

        return Ok(eventRepository.FindByCondition(e => e.Start >= startFilter.ToUniversalTime() && e.End <= endFilter
                                                      .ToUniversalTime()));
    }

    [Authorize]
    [HttpPost(Name = nameof(CreateEvent))]
    public Task<IActionResult> CreateEvent(EventCreateRequestDTO e) {
        
        throw new NotImplementedException();
    }
}