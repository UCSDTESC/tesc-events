using System.ComponentModel.DataAnnotations;
using AutoMapper;
using FluentValidation;
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
    private readonly IValidator<Event> validator;

    public EventsController(IEventRepository eventRepository, IMapper mapper, IValidator<Event> validator) {
        this.eventRepository = eventRepository;
        this.mapper = mapper;
        this.validator = validator;
    }
    
    [HttpGet(Name = nameof(GetEvents))]
    public IActionResult GetEvents(string? start = "", string? end = "") {
        if (!DateTime.TryParse(start, out var startFilter)) {
            startFilter = DateTime.Now;
        }

        if (!DateTime.TryParse(end, out var endFilter)) {
            endFilter = DateTime.MaxValue;
        }

        return Ok(eventRepository.FindByCondition(e => e.Start >= startFilter.ToUniversalTime() 
                                                       && e.End <= endFilter.ToUniversalTime()));
    }

    [Authorize]
    [HttpPost(Name = nameof(CreateEvent))]
    public async Task<IActionResult> CreateEvent([Required] [FromForm] EventCreateRequestDTO e) {
        var eventEntity = mapper.Map<Event>(e);
        var validationResult = await validator.ValidateAsync(eventEntity);

        if (!validationResult.IsValid) return BadRequest(
                                                         validationResult.Errors.Select(error => error.ErrorMessage));
        
        // TODO: Abstract into service transaction and async call
        eventRepository.Create(eventEntity);
        // TODO: Upload image to AWS
        eventRepository.Save();

        var eventResponse = mapper.Map<EventPublicResponseDTO>(eventEntity);
        return CreatedAtRoute(nameof(CreateEvent), new { Id = eventResponse.Id }, eventResponse);
    }
}