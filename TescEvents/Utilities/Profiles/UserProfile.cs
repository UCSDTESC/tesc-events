using AutoMapper;
using TescEvents.DTOs;
using TescEvents.DTOs.Users;
using TescEvents.Models;

namespace TescEvents.Utilities.Profiles; 

public class UserProfile : Profile {
    public UserProfile() {
        CreateMap<UserCreateRequestDTO, User>()
            .ForMember(u => u.PasswordHash, option => option.Ignore())
            .ForMember(u => u.Salt, option => option.Ignore());

        CreateMap<User, UserResponseDTO>();
    }
}