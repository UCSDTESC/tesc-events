using Microsoft.EntityFrameworkCore;
using Moq;
using TescEvents.Entities;
using TescEvents.Models;
using TescEvents.Repositories;

namespace Tests.UnitTests; 

public class EventTest : IDisposable {
    private readonly RepositoryContext context;
    private readonly IEventRepository repo;
    
    public EventTest() {
        var options = new DbContextOptionsBuilder()
                      .UseInMemoryDatabase(databaseName: "tesc-events")
                      .Options;

        context = new RepositoryContext(options);
        repo = new EventRepository(context);
    }
    
    [Fact]
    public void TestGetSingleEvent() {
        // Arrange
        var guid = Guid.NewGuid();
        var pastEvent = new Event {
            Id = guid,
            Title = "Past Event",
            Start = new DateTime(2021, 10, 12, 10, 0, 0).ToUniversalTime(),
            End = new DateTime(2021, 10, 12, 14, 0, 0).ToUniversalTime(),
        };

        context.Events.Add(pastEvent);
        context.SaveChanges();
        
        // Act
        Event? e = repo.GetEventByUuid(guid);
        Event? nonexistentEvent = repo.GetEventByUuid(Guid.NewGuid());

        // Assert
        Assert.NotNull(e);
        Assert.Equal(guid, e!.Id);
        
        Assert.Null(nonexistentEvent);
    }
    
    [Fact]
    public void TestCreateEvent() {
        // Arrange
        var guid = Guid.NewGuid();
        var e = new Event {
            Id = guid,
            Title = "New Event",
            Start = DateTime.UtcNow,
            End = DateTime.UtcNow.AddDays(1),
        };

        // Act
        repo.Create(e);
        repo.Save();
        
        // Assert
        var createdEvent = context.Events.Find(guid);
        
        Assert.NotNull(createdEvent);
    }

    [Fact]
    public void TestGetEventsWithinRange() {
        // Arrange
        var guid1 = Guid.NewGuid();
        var guid2 = Guid.NewGuid();
        var guid3 = Guid.NewGuid();

        var events = new[] {
            new Event {
                Id = guid1,
                Title = "Past Event",
                Start = DateTime.UtcNow.AddDays(-20),
                End = DateTime.UtcNow.AddDays(-19),
            },
            new Event {
                Id = guid2,
                Title = "Future Event",
                Start = DateTime.UtcNow.AddDays(10),
                End = DateTime.UtcNow.AddDays(10.5)
            },
            new Event {
                Id = guid3,
                Title = "Current Event",
                Start = DateTime.UtcNow.AddDays(-1),
                End = DateTime.UtcNow.AddDays(1),
            },
        };
        
        context.Events.AddRange(events);
        context.SaveChanges();
        
        // Act
        var all = repo.GetAllEventsWithinRange(DateTime.MinValue, DateTime.MaxValue).ToList();
        var pastTilNow = repo.GetAllEventsWithinRange(DateTime.MinValue, DateTime.UtcNow).ToList();
        var future = repo.GetAllEventsWithinRange(DateTime.UtcNow, DateTime.MaxValue).ToList();
        
        // Assert
        Assert.Collection(all, 
                          e => Assert.Equal(guid1, e.Id),
                          e => Assert.Equal(guid3, e.Id),
                          e => Assert.Equal(guid2, e.Id));
        Assert.Collection(pastTilNow, 
                          e => Assert.Equal(guid1, e.Id),
                          e => Assert.Equal(guid3, e.Id));
        Assert.Collection(future,
                          e => Assert.Equal(guid2, e.Id));
    }

    public void Dispose() {
        context.Database.EnsureDeleted();
        context.Dispose();
    }
}