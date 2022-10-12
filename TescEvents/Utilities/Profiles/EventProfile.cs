using AutoMapper;
using TescEvents.DTOs.Events;
using TescEvents.Models;

namespace TescEvents.Utilities.Profiles; 

public class EventProfile : Profile {
    public EventProfile() {
        CreateMap<EventCreateRequestDTO, Event>()
            .ForMember(e => e.Thumbnail, option => option.Ignore())
            .ForMember(e => e.Cover, option => option.Ignore());
        CreateMap<Event, EventPublicResponseDTO>();
    }
}