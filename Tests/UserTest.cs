using System.Security.Claims;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using TescEvents.Controllers;
using TescEvents.DTOs;
using TescEvents.Entities;
using TescEvents.Models;
using TescEvents.Profiles;
using TescEvents.Services;
using TescEvents.Validators;

namespace Tests;

public class UserTest : IDisposable {
    private UsersController usersController;
    private IMapper mapper;
    private IAuthService authService;
    private Mock<IUserService> userService;
    private IUploadService uploadService;
    private IEmailService emailService;
    private IValidator<UserCreateRequestDTO> userValidator;
    
    public UserTest() {
        var options = new DbContextOptionsBuilder()
                      .UseInMemoryDatabase(databaseName: "tesc-events")
                      .Options;

        var context = new RepositoryContext(options);

        authService = new AuthService();
        uploadService = new UploadService();
        emailService = new EmailService();
        userService = new Mock<IUserService>();
        userValidator = new UserCreateRequestValidator();
        mapper = new MapperConfiguration(c => {
            c.AddProfile<UserProfile>();
        }).CreateMapper();
        
        
        
        usersController = new UsersController(mapper, authService, userService.Object, uploadService, emailService, userValidator);
    }
    
    [Fact]
    public void TestUserRegistrationDefault() {
        // Arrange
        User? createdUser = null;
        var userDto = new UserCreateRequestDTO {
            Email = "shanekim28@gmail.com",
            Password = "supersecretpassword",
            Pid = "A16388796",
            First = "Shane",
            Last = "Kim",
        };
        userService
            .Setup(m => m.CreateUser(It.IsAny<User>()))
            .Callback<User>(u => createdUser = u);
        userService
            .Setup(m => m.GetUserByEmail(It.IsAny<string>()))
            .Returns((User?)null);
        
        // Act
        var result = usersController.Register(userDto);

        // Assert
        userService.Verify(e => e.CreateUser(It.IsAny<User>()), Times.Once);
        Assert.IsType<NoContentResult>(result);
        Assert.Equal(userDto.Email, createdUser.Email);
        Assert.Equal(userDto.Pid, createdUser.Pid);
        Assert.Equal(userDto.First, createdUser.First);
        Assert.Equal(userDto.Last, createdUser.Last);
        Assert.Equal(userDto.Phone, createdUser.Phone);
        Assert.Null(createdUser.ResetToken);
        Assert.Null(createdUser.ResumeUrl);
        Assert.Null(createdUser.BatchId);
    }

    [Fact]
    public void TestMissingFields() {
        // Arrange
        // Missing email
        var userDto = new UserCreateRequestDTO {
            Password = "supersecretpassword",
            Pid = "A16388796",
            First = "Shane",
            Last = "Kim",
        };

        userService
            .Setup(m => m.CreateUser(It.IsAny<User>()));
        userService
            .Setup(m => m.GetUserByEmail(It.IsAny<string>()))
            .Returns(null as User);

        // Act
        var result = usersController.Register(userDto);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public void TestUserDuplicateRegistration() {
        // Arrange
        const string existingUserEmail = "shanekim28@gmail.com";
        var existingUser = new UserCreateRequestDTO {
            Email = existingUserEmail,
            Password = "supersecretpassword",
            Pid = "A16388796",
            First = "Shane",
            Last = "Kim",
        }; 
        
        userService
            .Setup(m => m.GetUserByEmail(existingUserEmail))
            .Returns(new User());
        
        // Act
        var result = usersController.Register(existingUser);
        
        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public void TestUpdateInformationSimple() {
        // Arrange
        var fakeUser = new User {
            Id = new Guid(),
            First = "Shane",
            Last = "Kim",
            Phone = "8888888888"
        };
        usersController.ControllerContext.HttpContext = new DefaultHttpContext {
            User = new ClaimsPrincipal(new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.Actor, fakeUser.Id.ToString())
            }))
        };

        const string newFirst = "Shawn";
        const string newPhone = "8588888888";
        var userPatches = new JsonPatchDocument<UserPatches>();
        userPatches.Replace(e => e.First, newFirst);
        userPatches.Replace(e => e.Phone, newPhone);

        userService.Setup(e => e.GetUser(fakeUser.Id))
                   .Returns(fakeUser);

        // Act
        var result = usersController.UpdateUserInfo(fakeUser.Id, userPatches);

        // Assert
        Assert.IsType<OkObjectResult>(result);

        var okResult = result as OkObjectResult;
        Assert.NotNull(okResult);

        var userDto = okResult!.Value as UserResponseDTO;
        Assert.NotNull(userDto);
        
        Assert.Equal(newFirst, userDto!.First);
        Assert.Equal(newPhone, userDto.Phone);
    }

    [Fact]
    public void TestUpdatingUserAddingFieldRejects() {
        // Arrange
        var fakeUser = new User {
            Id = new Guid(),
            First = "Shane",
            Last = "Kim",
            Phone = "8888888888"
        };
        usersController.ControllerContext.HttpContext = new DefaultHttpContext {
            User = new ClaimsPrincipal(new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.Actor, fakeUser.Id.ToString())
            }))
        };

        var userPatches = new JsonPatchDocument<UserPatches>();
        userPatches.Operations.Add(new("add", "/middle", "", "middle"));

        userService.Setup(e => e.GetUser(fakeUser.Id))
                   .Returns(fakeUser);

        // Act
        var result = usersController.UpdateUserInfo(fakeUser.Id, userPatches); 
        
        // Assert
        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public void TestUpdatingUserRemovingFieldRejects() {
        // Arrange
        var fakeUser = new User {
            Id = new Guid(),
            First = "Shane",
            Last = "Kim",
            Phone = "8888888888"
        };
        usersController.ControllerContext.HttpContext = new DefaultHttpContext {
            User = new ClaimsPrincipal(new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.Actor, fakeUser.Id.ToString())
            }))
        };

        var userPatches = new JsonPatchDocument<UserPatches>();
        userPatches.Remove(e => e.First);

        userService.Setup(e => e.GetUser(fakeUser.Id))
                   .Returns(fakeUser);

        // Act
        var result = usersController.UpdateUserInfo(fakeUser.Id, userPatches);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public void TestUpdatingUserReplacingPreviouslyNullFieldAccepts() {
        // Arrange
        var fakeUser = new User {
            Id = new Guid(),
            First = "Shane",
            Last = "Kim",
        };
        usersController.ControllerContext.HttpContext = new DefaultHttpContext {
            User = new ClaimsPrincipal(new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.Actor, fakeUser.Id.ToString())
            }))
        };

        const string newPhone = "8989841234";
        var userPatches = new JsonPatchDocument<UserPatches>();
        userPatches.Add(e => e.Phone, newPhone);

        userService.Setup(e => e.GetUser(fakeUser.Id))
                   .Returns(fakeUser);

        // Act
        var result = usersController.UpdateUserInfo(fakeUser.Id, userPatches);

        // Assert
        var okResult = result as OkObjectResult;
        Assert.NotNull(okResult);

        var userDto = okResult!.Value as UserResponseDTO;
        Assert.NotNull(userDto);

        Assert.Equal(newPhone, userDto!.Phone); 
    }

    [Fact]
    public void TestUpdatingUserSettingPrivateFieldRejects() {
        // Arrange
        var fakeUser = new User {
            Id = new Guid(),
            First = "Shane",
            Last = "Kim",
        };
        usersController.ControllerContext.HttpContext = new DefaultHttpContext {
            User = new ClaimsPrincipal(new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.Actor, fakeUser.Id.ToString())
            }))
        };

        var userPatches = new JsonPatchDocument<UserPatches>();
        userPatches.Operations.Add(new("replace", "/batchId", "", "123"));

        userService.Setup(e => e.GetUser(fakeUser.Id))
                   .Returns(fakeUser);

        // Act
        var result = usersController.UpdateUserInfo(fakeUser.Id, userPatches);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }

    public void Dispose() {
    }
}