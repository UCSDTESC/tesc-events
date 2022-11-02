using AutoMapper;
using Moq;
using TescEvents.Controllers;
using TescEvents.Repositories;
using TescEvents.Services;
using TescEvents.Utilities.Profiles;
using TescEvents.Validators;

namespace Tests.UnitTests; 

public class EventsControllerTest {
    private readonly Mock<IEventRepository> mockEventRepo;
    private readonly Mock<IEventRegistrationRepository> mockRegistrationRepo;
    private readonly Mock<IStudentRepository> mockStudentRepo;
    private readonly Mock<IUploadService> mockUploadService;
    private readonly EventValidator eventValidator;
    private readonly IMapper mapper;
    
    private readonly EventsController controller;
    
    public EventsControllerTest() {
        mapper = new MapperConfiguration(c => {
            c.AddProfile<EventProfile>();
        }).CreateMapper();

        mockEventRepo = new Mock<IEventRepository>();
        mockRegistrationRepo = new Mock<IEventRegistrationRepository>();
        mockUploadService = new Mock<IUploadService>();
        mockStudentRepo = new Mock<IStudentRepository>();
        eventValidator = new EventValidator();

        controller = new EventsController(mockEventRepo.Object, 
                                          mockRegistrationRepo.Object, 
                                          mapper, 
                                          eventValidator,
                                          mockStudentRepo.Object, 
                                          mockUploadService.Object);
    }

    [Fact]
    public async Task TestGetEventsDefaultShowsAllFuture() {
        // Arrange
        
        // Act
        
        
        // Assert
    }

    [Fact]
    public async Task TestGetEventsFromStartDate() {
        
    }

    [Fact]
    public async Task TestGetEventsBeforeEndDate() {
        
    }
}