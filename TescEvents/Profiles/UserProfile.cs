using AutoMapper;
using TescEvents.DTOs;
using TescEvents.Models;

namespace TescEvents.Profiles; 

public class UserProfile : Profile {
    public UserProfile() {
        CreateMap<UserCreateRequestDTO, User>()
            .ForMember(u => u.PasswordHash, o => o.Ignore())
            .ForMember(u => u.Salt, o => o.Ignore());
    }
}