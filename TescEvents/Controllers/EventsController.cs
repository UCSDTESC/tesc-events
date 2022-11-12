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
    private readonly IUploadService uploadService;

    public EventsController(IEventRepository eventRepository, 
                            IEventRegistrationRepository registrationRepository, 
                            IMapper mapper, 
                            IValidator<Event> validator, 
                            IStudentRepository studentRepository, 
                            IUploadService uploadService) {
        this.eventRepository = eventRepository;
        this.registrationRepository = registrationRepository;
        this.mapper = mapper;
        this.validator = validator;
        this.studentRepository = studentRepository;
        this.uploadService = uploadService;
    }
    
    [HttpGet(Name = nameof(GetEvents))]
    public IActionResult GetEvents(string? start = "", string? end = "") {
        if (!DateTime.TryParse(start, out var startFilter)) {
            startFilter = DateTime.Now;
        }

        if (!DateTime.TryParse(end, out var endFilter)) {
            endFilter = DateTime.MaxValue;
        }

        return Ok(eventRepository
                      .GetAllEventsWithinRange(startFilter.ToUniversalTime(), 
                                               endFilter.ToUniversalTime()));
    }

    [Authorize]
    [HttpPost(Name = nameof(CreateEvent))]
    public async Task<IActionResult> CreateEvent([Required] [FromForm] EventCreateRequestDTO e) {
        var eventEntity = mapper.Map<Event>(e);
        var validationResult = await validator.ValidateAsync(eventEntity);

        if (!validationResult.IsValid) return BadRequest(
                                                         validationResult.Errors.Select(error => error.ErrorMessage));
        
        eventRepository.Create(eventEntity);
        // Upload image to AWS
        if (e.Thumbnail != null)
            uploadService.UploadFileToPath(e.Thumbnail, "");
        if (e.Cover != null)
            uploadService.UploadFileToPath(e.Cover, "");
        eventRepository.Save();

        var eventResponse = mapper.Map<EventPublicResponseDTO>(eventEntity);
        return CreatedAtRoute(nameof(CreateEvent), new { Id = eventResponse.Id }, eventResponse);
    }

    [Authorize]
    [HttpPost("event/{eventId}/register", Name = nameof(RegisterForEvent))]
    public async Task<IActionResult> RegisterForEvent(string eventId) {
        var _event = eventRepository.GetEventByUuid(Guid.Parse(eventId));
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