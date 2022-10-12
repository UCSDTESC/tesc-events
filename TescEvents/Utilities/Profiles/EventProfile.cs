using AutoMapper;
using TescEvents.DTOs.Events;
using TescEvents.Models;

namespace TescEvents.Utilities.Profiles; 

public class EventProfile : Profile {
    public EventProfile() {
        CreateMap<EventCreateRequestDTO, Event>();
        CreateMap<Event, EventPublicResponseDTO>();
    }
}