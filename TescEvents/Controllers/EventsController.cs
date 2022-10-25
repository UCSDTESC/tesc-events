using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TescEvents.DTOs;
using TescEvents.DTOs.Events;
using TescEvents.Models;
using TescEvents.Repositories;
using TescEvents.Services;

namespace TescEvents.Controllers; 

[ApiController]
[Route("/api/[controller]")]
public class EventsController : ControllerBase {
    private readonly IEventRepository eventRepository;
    private readonly IEventRegistrationRepository registrationRepository;
    private readonly IStudentRepository studentRepository;
    private readonly IMapper mapper;
    private readonly IValidator<Event> validator;

    public EventsController(IEventRepository eventRepository, 
                            IEventRegistrationRepository registrationRepository, 
                            IMapper mapper, 
                            IValidator<Event> validator, IStudentRepository studentRepository) {
        this.eventRepository = eventRepository;
        this.registrationRepository = registrationRepository;
        this.mapper = mapper;
        this.validator = validator;
        this.studentRepository = studentRepository;
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

    [Authorize]
    [HttpPost("event/{eventId}/register", Name = nameof(RegisterForEvent))]
    public async Task<IActionResult> RegisterForEvent(string eventId) {
        var _event = eventRepository.FindByCondition(e => e.Id == Guid.Parse(eventId))
                                    .FirstOrDefault();
        if (_event == null) return NotFound();

        var studentId = HttpContext.User.FindFirstValue(ClaimTypes.Actor);
        if (studentId == null) return Unauthorized();
        var student = studentRepository.GetUserByUuid(Guid.Parse(studentId));
        if (student == null) return Unauthorized();
        
        registrationRepository.RegisterStudentForEvent(student, _event);
        // TODO: Send registration email confirmations

        return Ok();
    }
}