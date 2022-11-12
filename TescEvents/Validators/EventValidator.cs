using FluentValidation;
using TescEvents.Models;

namespace TescEvents.Validators; 

public class EventValidator : AbstractValidator<Event> {
    public EventValidator() {
        RuleFor(e => e.Title)
            .NotEmpty();
        RuleFor(e => e.Start)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .GreaterThan(DateTime.Now);
        RuleFor(e => e.End)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .GreaterThan(e => e.Start);
        RuleFor(e => e.Description)
            .NotEmpty();
    }
}