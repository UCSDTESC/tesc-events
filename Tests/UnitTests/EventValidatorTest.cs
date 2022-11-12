using FluentValidation.TestHelper;
using TescEvents.Models;
using TescEvents.Validators;

namespace Tests.UnitTests; 

public class EventValidatorTest {
    private readonly EventValidator eventValidator;

    public EventValidatorTest() {
        eventValidator = new EventValidator();
    }
    
    [Fact]
    public void TestEventBasic() {
        // Arrange
        var e = new Event {
            Title = "New Event",
            Description = "Event description",
            Start = DateTime.Now.AddDays(1),
            End = DateTime.Now.AddDays(1).AddHours(1),
        };

        // Act
        var result = eventValidator.TestValidate(e);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(e => e.Title);
        result.ShouldNotHaveValidationErrorFor(e => e.Description);
        result.ShouldNotHaveValidationErrorFor(e => e.Start);
        result.ShouldNotHaveValidationErrorFor(e => e.End);
    }

    private static readonly DateTime eventStart = DateTime.Now.AddHours(1);
    public static readonly object[][] EndBeforeStartData = {
        new object[] { eventStart, eventStart },
        new object[] { eventStart, eventStart.AddSeconds(-1) },
        new object[] { eventStart, DateTime.UnixEpoch },
    };
    [Theory, MemberData(nameof(EndBeforeStartData))]
    public void TestEndIsBeforeOrEqualToStart(DateTime start, DateTime end) {
        // Arrange
        var e = new Event {
            Title = "New Event",
            Description = "Event description",
            Start = start,
            End = end,
        };
        
        // Act
        var result = eventValidator.TestValidate(e);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(e => e.Start);
        result.ShouldHaveValidationErrorFor(e => e.End);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void TestEventTitleEmptyOrNull(string title) {
        // Arrange
        var e = new Event {
            Title = title,
            Description = "Event description",
            Start = DateTime.Now.AddHours(1),
            End = DateTime.Now.AddHours(2),
        };
        
        // Act
        var result = eventValidator.TestValidate(e);
        
        // Assert
        result.ShouldHaveValidationErrorFor(e => e.Title);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void TestEventDescriptionEmptyOrNull(string description) {
        // Arrange
        var e = new Event {
            Title = "New Event",
            Description = description,
            Start = DateTime.Now.AddHours(1),
            End = DateTime.Now.AddHours(2),
        };
        
        // Act
        var result = eventValidator.TestValidate(e);
        
        // Assert
        result.ShouldHaveValidationErrorFor(e => e.Description);
    }
}