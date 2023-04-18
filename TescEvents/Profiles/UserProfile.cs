using AutoMapper;
using TescEvents.DTOs;
using TescEvents.Models;

namespace TescEvents.Profiles; 

public class UserProfile : Profile {
    public UserProfile() {
        CreateMap<StudentCreateRequestDTO, Student>();
        CreateMap<Student, StudentResponseDTO>();
        CreateMap<StudentPatches, Student>();
        CreateMap<Student, StudentPatches>();
    }
}