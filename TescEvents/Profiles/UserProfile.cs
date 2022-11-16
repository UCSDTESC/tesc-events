using AutoMapper;
using TescEvents.DTOs;
using TescEvents.Models;

namespace TescEvents.Profiles; 

public class UserProfile : Profile {
    public UserProfile() {
        CreateMap<UserCreateRequestDTO, User>();
        CreateMap<User, UserResponseDTO>();

        CreateMap<UserPatches, User>();
        CreateMap<User, UserPatches>();
    }
}