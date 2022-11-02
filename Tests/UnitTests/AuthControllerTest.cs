using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TescEvents.Controllers;
using TescEvents.DTOs.Users;
using TescEvents.Models;
using TescEvents.Repositories;
using TescEvents.Utilities.Profiles;
using TescEvents.Validators;

namespace Tests.UnitTests; 

public class AuthControllerTest {
    private readonly Mock<IStudentRepository> mockStudentRepo;
    private readonly IMapper mapper;
    private readonly StudentValidator studentValidator;
    private readonly AuthController controller;

    public AuthControllerTest() {
        mapper = new MapperConfiguration(c => {
            c.AddProfile<UserProfile>();
        }).CreateMapper();

        mockStudentRepo = new Mock<IStudentRepository>();
        studentValidator = new StudentValidator(mockStudentRepo.Object);
        controller = new AuthController(mockStudentRepo.Object, mapper, studentValidator);
    }
    
    [Fact]
    public async Task TestRegisterStudentMinimumInformation() {
        // Arrange
        var user = new UserCreateRequestDTO {
            FirstName = "Shane",
            LastName = "Kim",
            Password = "supersecretpassword",
            Username = "sek007@ucsd.edu"
        };
        
        // Act
        var result = await controller.RegisterUser(user);

        // Assert
        Assert.IsType<CreatedAtRouteResult>(result);
    }

    [Fact]
    public async Task TestRegisterStudentDuplicateUsername() { 
        // Arrange
        var user1 = new UserCreateRequestDTO {
            FirstName = "Shane",
            LastName = "Kim",
            Password = "supersecretpassword",
            Username = "sek007@ucsd.edu"
        };
        var user2 = new UserCreateRequestDTO {
            FirstName = "Shane",
            LastName = "Kim",
            Password = "supersecretpassword",
            Username = "sek008@ucsd.edu"
        };

        mockStudentRepo.Setup(
                              repo => repo.GetUserByUsername(It.Is<string>(u => u == user1.Username)))
                       .Returns(new Student());

        // Act
        var result1 = await controller.RegisterUser(user1);
        var result2 = await controller.RegisterUser(user2);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result1);
        Assert.IsType<CreatedAtRouteResult>(result2);
    }
}