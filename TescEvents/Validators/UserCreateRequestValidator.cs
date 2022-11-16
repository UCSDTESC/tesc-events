using FluentValidation;
using TescEvents.DTOs;

namespace TescEvents.Validators; 

public class UserCreateRequestValidator : AbstractValidator<UserCreateRequestDTO> {
    public UserCreateRequestValidator() {
        RuleLevelCascadeMode = CascadeMode.Stop;
        
        RuleFor(u => u.Email)
            .EmailAddress();
        RuleFor(u => u.Password)
            .NotEmpty()
            .MinimumLength(8);
        RuleFor(u => u.Pid)
            .NotEmpty();
        RuleFor(u => u.First)
            .NotEmpty();
        RuleFor(u => u.Last)
            .NotEmpty();
    }
}